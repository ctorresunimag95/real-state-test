namespace RealState.Test.Application.Property.UpdatePropertyInfo;

public record UpdatePropertyInfoCommand(Guid IdProperty
    ,string Name
    , string Address
    , string CodeInternal
    , int Year);