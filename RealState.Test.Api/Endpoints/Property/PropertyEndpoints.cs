using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RealState.Test.Api.Common.Versioning;
using RealState.Test.Api.Endpoints.Property.AddImage;
using RealState.Test.Api.Endpoints.Property.ChangePrice;
using RealState.Test.Api.Endpoints.Property.CreateProperty;
using RealState.Test.Api.Endpoints.Property.ListProperties;
using RealState.Test.Api.Endpoints.Property.UpdatePropertyInfo;
using RealState.Test.Application.Property.AddImage;
using RealState.Test.Application.Property.ChangePrice;
using RealState.Test.Application.Property.Create;
using RealState.Test.Application.Property.ListProperties;
using RealState.Test.Application.Property.UpdatePropertyInfo;
using RealState.Test.Domain.Property;
using PropertyImageResponse = RealState.Test.Api.Endpoints.Property.ListProperties.PropertyImageResponse;
using PropertyOwnerResponse = RealState.Test.Api.Endpoints.Property.ListProperties.PropertyOwnerResponse;

namespace RealState.Test.Api.Endpoints.Property;

internal static class PropertyEndpoints
{
    public static void MapPropertyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/properties")
            .WithApiVersionSet(Versioning.VersionSet)
            .HasApiVersion(1, 0);

        group.MapPost(string.Empty, CreateProperty);
        
        group.MapGet(string.Empty, GetProperties);

        group.MapPut("/{idProperty:guid}/change-price", ChangePrice);

        group.MapPut("/{idProperty:guid}", UpdatePropertyInfo);

        group.MapPost("/{idProperty:guid}/add-image", AddImage)
            .DisableAntiforgery();
    }

    public static async Task<Results<Created<CreatePropertyResponse>, ProblemHttpResult>> CreateProperty(
        [FromBody] CreatePropertyRequest request,
        [FromServices] ICreatePropertyHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new CreatePropertyCommand(request.Name, request.Address, request.Price, request.CodeInternal,
            request.Year,
            request.IdOwner);

        var propertyId = await handler.HandleAsync(command, cancellationToken);

        var response = new CreatePropertyResponse(propertyId,
            request.Name, request.Address, request.Price, request.CodeInternal, request.Year, request.IdOwner);

        return TypedResults.Created($"/properties/{propertyId}", response);
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> ChangePrice(
        [FromRoute] Guid idProperty,
        [FromBody] ChangePropertyPriceRequest request
        , [FromServices] IChangePropertyPriceHandler handler
        , CancellationToken cancellationToken = default)
    {
        var command = new ChangePropertyPriceCommand(idProperty, request.Price);

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> UpdatePropertyInfo(
        [FromRoute] Guid idProperty,
        [FromBody] UpdatePropertyInfoRequest request
        , [FromServices] IUpdatePropertyInfoHandler handler
        , CancellationToken cancellationToken = default)
    {
        var command = new UpdatePropertyInfoCommand(idProperty, request.Name, request.Address,
            request.CodeInternal, request.Year);

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.NoContent();
    }

    public static async Task<Results<Ok<AddPropertyImageResponse>, ProblemHttpResult>> AddImage(
        [FromRoute] Guid idProperty,
        [FromForm(Name = "file")] IFormFile file,
        [FromServices] IAddImageHandler handler,
        CancellationToken cancellationToken = default)
    {
        using var stream = file.OpenReadStream();

        var command = new AddImageCommand(idProperty, file.FileName, stream);

        var response = await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok(new AddPropertyImageResponse(idProperty, response));
    }

    public static async Task<Results<Ok<IEnumerable<PropertyResponse>>, ProblemHttpResult>> GetProperties(
        [AsParameters] PropertyFilters filters
        , [FromServices] IListPropertiesHandler handler
        , CancellationToken cancellationToken = default)
    {
        var query = new GetPropertiesQuery(filters.Name, filters.Address, filters.CodeInternal, filters.Price,
            filters.Year, filters.IdOwner);

        var properties = await handler.HandleAsync(query, cancellationToken);

        var mappedResults = properties.Select(x =>
            new PropertyResponse(x.IdProperty, x.Name, x.Address, x.Price, x.CodeInternal, x.Year
                , new PropertyOwnerResponse(x.Owner.IdOwner, x.Owner.Name, x.Owner.Address)
                , x.Images.Select(i => new PropertyImageResponse(i.IdPropertyImage, i.Url, i.Enabled))));
        
        return TypedResults.Ok(mappedResults);
    }
}