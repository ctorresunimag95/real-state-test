namespace RealState.Test.Domain.Property;

public class Property
{
    public Guid IdProperty { get; private set; }
    
    public string Name { get; private set; }
    
    public string Address { get; private set; }
    
    public decimal Price { get; private set; }
    
    public string CodeInternal { get; private set; }
    
    public int Year { get; private set; }

    public Guid IdOwner { get; private set; }
    
    public Owner Owner { get; }
    
    public ICollection<PropertyImage> PropertyImages { get; } = new List<PropertyImage>();
    
    public ICollection<PropertyTrace> PropertyTraces { get; } = new List<PropertyTrace>();

    private Property()
    {
    }

    public static Property Create(string name, string address, decimal price, string codeInternal, int year,
        Guid idOwner)
    {
        return new Property
        {
            IdProperty = Guid.CreateVersion7(),
            Name = name,
            Address = address,
            Price = price,
            CodeInternal = codeInternal,
            Year = year,
            IdOwner = idOwner
        };
    }
    
    public void AddImage(string file)
    {
        var image = PropertyImage.Create(IdProperty, file);
        PropertyImages.Add(image);
    }

    public void ChangePrice(decimal price)
    {
        if (price < 0)
        {
            throw new InvalidOperationException("Price cannot be negative");
        }
        Price = price;
    }

    public void UpdateInfo(string name, string address, string codeInternal, int year)
    {
        Name = name;
        Address = address;
        CodeInternal = codeInternal;
        Year = year;
    }
    
    public void AddTrace(DateTime dateSale, string name, decimal value, decimal tax)
    {
        var trace = PropertyTrace.Create(IdProperty, dateSale, name, value, tax);
        PropertyTraces.Add(trace);
    }
}