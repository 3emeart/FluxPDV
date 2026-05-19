using Exceptions;
using MassTransit;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Contracts;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.Application.Services;

public class VendaService : IVendaService
{
       private readonly IVendaRepository _vendaRepository;
       private readonly IProductRepository _productRepository;
       private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
       private readonly IPublishEndpoint _publishEndpoint;
       private readonly IItemVendaRepository _itemVendaRepository;
       private readonly IUnitOfWork _unitOfWork;

       public VendaService(
              IVendaRepository vendaRepository,
              IProductRepository productRepository,
              IMovimentacaoEstoqueRepository movimentacaoRepository,
              IPublishEndpoint publishEndpoint,
              IItemVendaRepository itemVendaRepository,
              IUnitOfWork unitOfWork)
       {
              _vendaRepository = vendaRepository;
              _productRepository = productRepository;
              _movimentacaoRepository = movimentacaoRepository;
              _publishEndpoint = publishEndpoint;
              _itemVendaRepository = itemVendaRepository;
              _unitOfWork = unitOfWork;
              
       }

       public async Task<VendaDetalheResponse> AbrirAsync(Guid OperadorId)
       {
              var venda = new Venda
              {
                     OperadorId = OperadorId,
                     Status = StatusVenda.Aberta,
                     TotalFinal = 0,
                     AbertaEm = DateTime.UtcNow,
              };

              await _vendaRepository.AddAsync(venda);
              await _unitOfWork.CommitAsync();

               return new VendaDetalheResponse(venda);
       }

       public async Task<Venda> AddItemAsync(Guid id, AddItemRequest request, Guid operadorId)
       {
              var vendasExistentes = await _vendaRepository.GetByIdAsync(id, true)
                                    ?? throw new KeyNotFoundException("Venda não encontrada");

              if (vendasExistentes.Status != StatusVenda.Aberta)
              {
                     throw new BusinessException("A venda não está aberta para adição de itens");
              }

              var produto = await _productRepository.FindByIdAsync(request.ProdutoId)
                            ?? throw new NotFoundException("Produto não encontrado");

              if (!produto.Ativo)
              {
                     throw new BusinessException("Produto não está ativo");
              }

              if (produto.Quantidade < request.Quantidade)
              {
                     throw new BusinessException("Estoque insuficiente");
              }

              var itemFinal = new ItemVenda(produto.Id, request.Quantidade, produto.PrecoVenda)
              {
                     VendaId = id
              };

              await _itemVendaRepository.AddAsync(itemFinal);

              var vendaAtualizada = await _vendaRepository.GetByIdAsync(id, true)
                     ?? throw new NotFoundException("Venda não encontrada após adicionar item");

              vendaAtualizada.TotalFinal = vendaAtualizada.Itens!.Sum(i => i.Subtotal);
              await _unitOfWork.CommitAsync();

              return vendaAtualizada;
       }

       public async Task<Venda> RemoveItemAsync(Guid id, Guid itemId)
       {
              var venda = await _vendaRepository.GetByIdAsync(id, true)
                          ?? throw new NotFoundException("Venda não encontrada");

              if (venda.Status != StatusVenda.Aberta)
              {
                     throw new BusinessException("Esta venda não foi aberta");
              }

              var item = venda.Itens.FirstOrDefault(i => i.Id == itemId);
              if (item == null)
              {
                     throw new NotFoundException("Este item nao pertence a essa venda");
              }

              venda.Itens.Remove(item);

              venda.TotalFinal = venda.Itens.Sum(i => i.PrecoUnitario * i.Quantidade);

              await _unitOfWork.CommitAsync();
              return venda;
       }

       public async Task<VendaDetalheResponse> FinalizarAsync(Guid vendaId, FinalizarVendaRequest request, Guid operadorId)
       {
              var venda = await _vendaRepository.GetByIdAsync(vendaId, true)
                          ?? throw new NotFoundException("Venda não encontrada");

              if (venda.Status != StatusVenda.Aberta)
              {
                     throw new BusinessException("Somente vendas com status 'Aberta' podem ser finalizadas");
              }

              if (venda.Itens == null || !venda.Itens.Any())
              {
                     throw new BusinessException("Não é possível finalizar uma venda sem itens");
              }

              foreach (var item in venda.Itens)
              {
                     var produto = await _productRepository.FindByIdAsync(item.ProdutoId)
                                   ?? throw new NotFoundException($"Produto {item.ProdutoId} não encontrado");

                     if (produto.Quantidade < item.Quantidade)
                     {
                            throw new BusinessException(
                                   $"Estoque insuficiente para o produto '{produto.Nome}'. " +
                                   $"Disponível: {produto.Quantidade}, Solicitado: {item.Quantidade}");
                     }
              }

              foreach (var item in venda.Itens)
              {
                     var produto = await _productRepository.FindByIdAsync(item.ProdutoId);

                     produto!.Quantidade -= item.Quantidade;

                     if (produto.Quantidade <= produto.EstoqueMinimo)
                     {
                            await _publishEndpoint.Publish(new EstoqueBaixoEvent(
                                   produto.Id,
                                   produto.Nome,
                                   produto.Quantidade,
                                   produto.EstoqueMinimo));
                     }

                     var movimentacao = new MovimentacaoEstoque
                     {
                            ProdutoId = produto.Id,
                            Quantidade = item.Quantidade,
                            Tipo = TipoMovimentacao.SaidaVenda,
                            VendaId = venda.Id,
                            UsuarioId = operadorId,
                            Observacao = $"Saída por venda #{venda.Id}"
                     };

                     await _movimentacaoRepository.AddAsync(movimentacao);
              }

              venda.Status = StatusVenda.Finalizado;
              venda.FormaPagamento = request.FormaPagamento;
              venda.FinalizadaEm = DateTime.UtcNow;

              await _unitOfWork.CommitAsync();

              return new VendaDetalheResponse(venda);
       }

       public async Task<VendaDetalheResponse> CancelarAsync(Guid vendaId, CancelarVendaRequest request, Guid gerenteId)
       {
              var venda = await _vendaRepository.GetByIdAsync(vendaId, true)
                          ?? throw new NotFoundException("Venda não encontrada");

              if (venda.Status == StatusVenda.Cancelado)
              {
                     throw new BusinessException("Esta venda já foi cancelada");
              }

              if (string.IsNullOrWhiteSpace(request.MotivoCancelamento))
              {
                     throw new BusinessException("O motivo do cancelamento é obrigatório");
              }

              if (venda.Status == StatusVenda.Finalizado && venda.Itens != null)
              {
                     foreach (var item in venda.Itens)
                     {
                            var produto = await _productRepository.FindByIdAsync(item.ProdutoId)
                                          ?? throw new NotFoundException($"Produto {item.ProdutoId} não encontrado");

                            produto.Quantidade += item.Quantidade;

                            var movimentacao = new MovimentacaoEstoque
                            {
                                   ProdutoId = produto.Id,
                                   Quantidade = item.Quantidade,
                                   Tipo = TipoMovimentacao.DevolucaoCancelamento,
                                   VendaId = venda.Id,
                                   UsuarioId = gerenteId,
                                   Observacao = $"Devolução por cancelamento da venda #{venda.Id}. Motivo: {request.MotivoCancelamento}"
                            };

                            await _movimentacaoRepository.AddAsync(movimentacao);
                     }
              }

              venda.Status = StatusVenda.Cancelado;
              venda.MotivoCancelamento = request.MotivoCancelamento;
              venda.CanceladaEm = DateTime.UtcNow;

              await _unitOfWork.CommitAsync();

              return new VendaDetalheResponse(venda);
       }

       public async Task<VendaDetalheResponse> GetByIdAsync(Guid vendaId)
       {
              var venda = await _vendaRepository.GetByIdAsync(vendaId, true)
                          ?? throw new NotFoundException("Venda não encontrada");

              return new VendaDetalheResponse(venda);
       }
}