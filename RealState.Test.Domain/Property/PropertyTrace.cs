namespace RealState.Test.Domain.Property;

public class PropertyTrace
{
    public Guid IdPropertyTrace { get; private set; }
    
    public DateTime DateSale { get; private set; }
    
    public string Name { get; private set; }
    
    public decimal Value { get; private set; }
    
    public decimal Tax { get; private set; }

    private PropertyTrace()
    {
    }

    public static PropertyTrace Create(DateTime dateSale, string name, decimal value, decimal tax)
    {
        var trace = new PropertyTrace();
        trace.DateSale = dateSale;
        trace.Name = name;
        trace.Value = value;
        trace.Tax = tax;
        
        return trace;
    }
}