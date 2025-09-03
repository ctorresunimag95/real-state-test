using FluentValidation;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.ChangePrice;

public interface IChangePropertyPriceHandler
{
    Task HandleAsync(ChangePropertyPriceCommand command, CancellationToken cancellationToken = default);
}

public class ChangePropertyPriceHandler : IChangePropertyPriceHandler
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IValidator<ChangePropertyPriceCommand> _validator;

    public ChangePropertyPriceHandler(IPropertyRepository propertyRepository,
        IValidator<ChangePropertyPriceCommand> validator)
    {
        _propertyRepository = propertyRepository;
        _validator = validator;
    }

    public async Task HandleAsync(ChangePropertyPriceCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var property = await _propertyRepository.GetById(command.IdProperty, cancellationToken);

        if (property is null)
        {
            throw new InvalidOperationException("Property not found");
        }

        property.ChangePrice(command.Price);
        await _propertyRepository.SaveAsync(cancellationToken);
    }
}