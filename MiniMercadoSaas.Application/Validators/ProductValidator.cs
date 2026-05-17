using FluentValidation;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.Services;

namespace MiniMercadoSaas.Application.Validators;

public class ProductValidator : AbstractValidator<ProdutoRequest>
{
    public ProductValidator()
    {
        RuleFor(produto => produto.Codigo)
            .NotEmpty().WithMessage("O código de barras é obrigatório")
            .Length(13).WithMessage("O código de barras deve conter exatamente 13 caracteres");

        RuleFor(produto => produto.Nome)
            .NotEmpty().WithMessage("O nome do produto deve ser informado");

        RuleFor(produto => produto.PrecoVenda)
            .GreaterThan(produto => produto.PrecoCompra)
            .WithMessage("O preço de venda deve ser maior do que o preço de compra");
        
        RuleFor(produto => produto.PrecoVenda)
            .NotEmpty().WithMessage("O preço de venda deve ser informado");

        RuleFor(produto => produto.PrecoCompra)
            .NotEmpty().WithMessage("O preço de compra deve ser informado");
        
        RuleFor(produto => produto.QuantidadeInicial)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero");
        
        
    } 
    
}