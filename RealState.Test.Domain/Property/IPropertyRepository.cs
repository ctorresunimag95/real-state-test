namespace RealState.Test.Domain.Property;

public interface IPropertyRepository
{
    public Task<Property?> GetById(Guid id, CancellationToken cancellationToken = default);

    public Task AddAsync(Property property, CancellationToken cancellationToken = default);

    Task UpdateAsync(Property property, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Property>>
        GetAsync(PropertyFilters filters, CancellationToken cancellationToken = default);
}

public record PropertyFilters(
    string? Name,
    string? Address,
    string? CodeInternal,
    decimal? Price,
    int? Year,
    Guid? IdOwner);