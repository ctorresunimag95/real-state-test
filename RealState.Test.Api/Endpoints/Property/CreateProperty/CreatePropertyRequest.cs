namespace RealState.Test.Api.Endpoints.Property.CreateProperty;

public record CreatePropertyRequest(string Name
    , string Address
    , decimal Price
    , string CodeInternal
    , int Year
    , Guid IdOwner);