using FluentValidation;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;

namespace MiniMercadoSaas.Application.Validators;

public class CategoryValidator : AbstractValidator<CategoriaCreateRequest>
{

    public CategoryValidator()
    {
        RuleFor(categoria => categoria.Nome).NotEmpty().MaximumLength(20).WithMessage("O campo {PropertyName} precisa ser fornecido");
        
        RuleFor(categoria => categoria.Descricao).MaximumLength(200).WithMessage("Máximo de 200 caracteres");
    }
    
}