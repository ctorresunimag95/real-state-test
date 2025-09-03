namespace RealState.Test.Application.Property.ListProperties;

public record GetPropertiesQuery(
    string? Name = null,
    string? Address = null,
    string? CodeInternal = null,
    decimal? Price = null,
    int? Year = null,
    Guid? IdOwner = null);