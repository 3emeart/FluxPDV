using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Domain.Interfaces;

public interface IPromocaoRepository
{
    Task AddAsync(Promocao promocao);
    Task <IEnumerable<Promocao>> ListarPromocaoAtivaAsync(DateTime data);
    Task <IEnumerable<Promocao>> GetAllAsync();
    Task <Promocao?> ObterPromocaoPorIdAsync(Guid id);
    void Update(Promocao promocao);
}