using System.Runtime.CompilerServices;

namespace RealState.Test.Domain.Property;

public class Owner
{
    public Guid IdOwner { get; private set; }
    
    public string Name { get; private set; }
    
    public string Address { get; private set; }
    
    public string Photo { get; private set; }
    
    public DateTime Birthday { get; private set; }
    
    public ICollection<Property> Properties { get; } = new List<Property>();

    private Owner()
    {
    }

    public static Owner Create(string name, string address, string photo, DateTime birthday)
    {
        return new Owner
        {
            IdOwner = Guid.CreateVersion7(),
            Name = name,
            Address = address,
            Photo = photo,
            Birthday = birthday
        };
    }
}