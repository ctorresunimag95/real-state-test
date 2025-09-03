namespace RealState.Test.Api.Endpoints.Property.ListProperties;

public record PropertyResponse(Guid IdProperty
    , string Name
    , string Address
    , decimal Price
    , string CodeInternal
    , int Year
    , PropertyOwnerResponse Owner
    , IEnumerable<PropertyImageResponse> Images);
    
public record PropertyOwnerResponse(Guid IdOwner, string Name, string Address);
    
public record PropertyImageResponse(Guid IdPropertyImage, string Url, bool Enabled);