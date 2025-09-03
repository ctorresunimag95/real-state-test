using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.ListProperties;

public interface IListPropertiesHandler
{
    Task<IReadOnlyCollection<GetPropertiesResponse>> HandleAsync(GetPropertiesQuery query,
        CancellationToken cancellationToken = default);
}

public class ListPropertiesHandler : IListPropertiesHandler
{
    private readonly IPropertyRepository _propertyRepository;

    public ListPropertiesHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<IReadOnlyCollection<GetPropertiesResponse>> HandleAsync(GetPropertiesQuery query,
        CancellationToken cancellationToken = default)
    {
        var filters = new PropertyFilters(query.Name, query.Address, query.CodeInternal, query.Price, query.Year,
            query.IdOwner);

        var properties = await _propertyRepository.GetAsync(filters, cancellationToken);

        return properties.Select(x => new GetPropertiesResponse(
                x.IdProperty
                , x.Name
                , x.Address
                , x.Price
                , x.CodeInternal
                , x.Year
                , new PropertyOwnerResponse(x.IdOwner, x.Owner.Name, x.Owner.Address)
                , x.PropertyImages.Select(i => new PropertyImageResponse(i.IdPropertyImage, i.File, i.Enabled))))
            .ToList()
            .AsReadOnly();
    }
}