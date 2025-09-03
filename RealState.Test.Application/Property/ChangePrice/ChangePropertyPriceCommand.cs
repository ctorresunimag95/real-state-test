namespace RealState.Test.Application.Property.ChangePrice;

public record ChangePropertyPriceCommand(Guid IdProperty, decimal Price);