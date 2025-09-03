using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.UpdatePropertyInfo;

public interface IUpdatePropertyInfoHandler
{
    Task HandleAsync(UpdatePropertyInfoCommand command, CancellationToken cancellationToken = default);
}

public class UpdatePropertyInfoHandler : IUpdatePropertyInfoHandler
{
    private readonly IPropertyRepository _propertyRepository;

    public UpdatePropertyInfoHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task HandleAsync(UpdatePropertyInfoCommand command, CancellationToken cancellationToken = default)
    {
        var property = await _propertyRepository.GetById(command.IdProperty, cancellationToken);

        if (property is null)
        {
            throw new InvalidOperationException("Property not found");
        }
        
        property.UpdateInfo(command.Name, command.Address, command.CodeInternal, command.Year);
        
        await _propertyRepository.SaveAsync(cancellationToken);
    }
}