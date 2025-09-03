namespace RealState.Test.Application.Property.Create;

public record CreatePropertyCommand(string Name
    , string Address
    , decimal Price
    , string CodeInternal
    , int Year
    , Guid IdOwner);