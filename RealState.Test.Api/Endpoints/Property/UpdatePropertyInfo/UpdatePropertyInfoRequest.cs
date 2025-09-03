namespace RealState.Test.Api.Endpoints.Property.UpdatePropertyInfo;

public record UpdatePropertyInfoRequest(string Name
    , string Address
    , string CodeInternal
    , int Year);