using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.ChangePrice;

public interface IChangePropertyPriceHandler
{
    Task HandleAsync(ChangePropertyPriceCommand command, CancellationToken cancellationToken = default);
}

public class ChangePropertyPriceHandler : IChangePropertyPriceHandler
{
    private readonly IPropertyRepository _propertyRepository;

    public ChangePropertyPriceHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task HandleAsync(ChangePropertyPriceCommand command, CancellationToken cancellationToken = default)
    {
        var property = await _propertyRepository.GetById(command.IdProperty, cancellationToken);

        if (property is null)
        {
            throw new InvalidOperationException("Property not found");
        }
        
        property.ChangePrice(command.Price);
        await _propertyRepository.SaveAsync(cancellationToken);
    }
}