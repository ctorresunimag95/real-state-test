using FluentValidation;

namespace RealState.Test.Application.Property.ChangePrice;

public record ChangePropertyPriceCommand(Guid IdProperty, decimal Price);

public sealed class ChangePropertyPriceCommandValidator : AbstractValidator<ChangePropertyPriceCommand>
{
    public ChangePropertyPriceCommandValidator()
    {
        RuleFor(x => x.IdProperty)
            .NotEmpty();
        
        RuleFor(x => x.Price)
            .GreaterThan(0);
        
    }
}