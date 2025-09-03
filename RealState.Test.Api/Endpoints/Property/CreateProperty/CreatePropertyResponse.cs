namespace RealState.Test.Api.Endpoints.Property.CreateProperty;

public record CreatePropertyResponse(
    Guid IdProperty
    , string Name
    , string Address
    , decimal Price
    , string CodeInternal
    , int Year
    , Guid IdOwner);