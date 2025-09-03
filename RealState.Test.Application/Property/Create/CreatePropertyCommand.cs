using FluentValidation;

namespace RealState.Test.Application.Property.Create;

public record CreatePropertyCommand(
    string Name,
    string Address,
    decimal Price,
    string CodeInternal,
    int Year,
    Guid IdOwner);

public sealed class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
    
        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(455);
    
        RuleFor(x => x.Price)
            .GreaterThan(0);
    
        RuleFor(x => x.CodeInternal)
            .NotEmpty()
            .MaximumLength(50);
    
        RuleFor(x => x.Year)
            .GreaterThan(1800)
            .LessThanOrEqualTo(DateTime.Now.Year);
    
        RuleFor(x => x.IdOwner)
            .NotEmpty();
    }
}