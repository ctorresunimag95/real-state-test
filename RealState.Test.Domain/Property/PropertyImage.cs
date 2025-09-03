namespace RealState.Test.Domain.Property;

public class PropertyImage
{
    public Guid IdPropertyImage { get; private set; }
    
    public string File { get; private set; }
    
    public bool Enabled { get; private set; }

    private PropertyImage()
    {
    }

    public static PropertyImage Create(string file)
    {
        var image = new PropertyImage();
        image.Enabled = true;
        image.File = file;
        image.File = file;
        
        return image;
    }
}