namespace RealState.Test.Domain.Property;

public class PropertyTrace
{
    public Guid IdPropertyTrace { get; private set; }
    
    public DateTime DateSale { get; private set; }
    
    public string Name { get; private set; }
    
    public decimal Value { get; private set; }
    
    public decimal Tax { get; private set; }
    
    public Guid IdProperty { get; private set; }

    private PropertyTrace()
    {
    }

    public static PropertyTrace Create(Guid idProperty, DateTime dateSale, string name, decimal value, decimal tax)
    {
        return new PropertyTrace
        {
            IdPropertyTrace = Guid.CreateVersion7(),
            DateSale = dateSale,
            Name = name,
            Value = value,
            Tax = tax,
            IdProperty = idProperty
        };
    }
}