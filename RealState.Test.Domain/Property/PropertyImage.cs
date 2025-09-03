namespace RealState.Test.Domain.Property;

public class PropertyImage
{
    public Guid IdPropertyImage { get; private set; }
    
    public string File { get; private set; }
    
    public bool Enabled { get; private set; }
    
    public Guid IdProperty { get; private set; }

    private PropertyImage()
    {
    }

    public static PropertyImage Create(Guid idProperty, string file)
    {
        return new PropertyImage
        {
            IdPropertyImage = Guid.CreateVersion7(),
            File = file,
            Enabled = true,
            IdProperty = idProperty
        };
    }
}