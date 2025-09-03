using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.Create;

public interface ICreatePropertyHandler
{
    Task<Guid> HandleAsync(CreatePropertyCommand command, CancellationToken cancellationToken = default);
}

public class CreatePropertyHandler : ICreatePropertyHandler
{
    private readonly IPropertyRepository _propertyRepository;

    public CreatePropertyHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<Guid> HandleAsync(CreatePropertyCommand command, CancellationToken cancellationToken = default)
    {
        var property = Domain.Property.Property.Create(command.Name
            , command.Address
            , command.Price
            , command.CodeInternal
            , command.Year
            , command.IdOwner);

        await _propertyRepository.AddAsync(property, cancellationToken);

        return property.IdProperty;
    }
}