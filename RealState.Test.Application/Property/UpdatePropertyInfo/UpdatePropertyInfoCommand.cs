using FluentValidation;

namespace RealState.Test.Application.Property.UpdatePropertyInfo;

public record UpdatePropertyInfoCommand(Guid IdProperty
    ,string Name
    , string Address
    , string CodeInternal
    , int Year);

public sealed class UpdatePropertyInfoCommandValidator : AbstractValidator<UpdatePropertyInfoCommand>
{
    public UpdatePropertyInfoCommandValidator()
    {
        RuleFor(x => x.IdProperty)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(455);

        RuleFor(x => x.CodeInternal)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Year)
            .GreaterThan(1800)
            .LessThanOrEqualTo(DateTime.Now.Year);
    }
}
    
    