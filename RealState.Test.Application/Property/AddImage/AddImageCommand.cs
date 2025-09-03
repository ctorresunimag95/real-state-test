using FluentValidation;

namespace RealState.Test.Application.Property.AddImage;

public record AddImageCommand(Guid IdProperty, string FileName, Stream Stream);

public sealed class AddImageCommandValidator : AbstractValidator<AddImageCommand>
{
    public AddImageCommandValidator()
    {
        RuleFor(x => x.IdProperty)
            .NotEmpty();

        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);
    }
}

