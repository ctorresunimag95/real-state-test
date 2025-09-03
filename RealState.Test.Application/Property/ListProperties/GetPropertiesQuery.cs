namespace RealState.Test.Application.Property.ListProperties;

public record GetPropertiesQuery(
    string? Name,
    string? Address,
    string? CodeInternal,
    decimal? Price,
    int? Year,
    Guid? IdOwner);