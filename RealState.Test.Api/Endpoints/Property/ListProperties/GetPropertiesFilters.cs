using Microsoft.AspNetCore.Mvc;

namespace RealState.Test.Api.Endpoints.Property.ListProperties;

public record GetPropertiesFilters(
    [FromQuery] string? Name,
    [FromQuery] string? Address,
    [FromQuery] string? CodeInternal,
    [FromQuery] decimal? Price,
    [FromQuery] int? Year,
    [FromQuery] Guid? IdOwner);