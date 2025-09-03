using RealState.Test.Domain.Property;

namespace RealState.Test.Domain.UnitTests.Property;

[TestFixture]
public class PropertyTests
{
    [Test]
    public void Create_ShouldInitializeProperty_WithValidValues()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg", new DateTime(1980, 1, 1));
        var name = "Test Property";
        var address = "123 Main St";
        var price = 100000m;
        var codeInternal = "INT-001";
        var year = 2020;

        // Act
        var property =
            RealState.Test.Domain.Property.Property.Create(name, address, price, codeInternal, year, owner.IdOwner);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(property.Name, Is.EqualTo(name));
            Assert.That(property.Address, Is.EqualTo(address));
            Assert.That(property.Price, Is.EqualTo(price));
            Assert.That(property.CodeInternal, Is.EqualTo(codeInternal));
            Assert.That(property.Year, Is.EqualTo(year));
            Assert.That(property.IdOwner, Is.EqualTo(owner.IdOwner));
        });
    }

    [Test]
    public void ChangePrice_ShouldUpdatePrice()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg",
            new DateTime(1980, 1, 1));
        var property = RealState.Test.Domain.Property.Property.Create("Test Property", "123 Main St", 100000m,
            "INT-001", 2020, owner.IdOwner);
        var newPrice = 120000m;

        // Act
        property.ChangePrice(newPrice);

        // Assert
        Assert.That(property.Price, Is.EqualTo(newPrice));
    }

    [Test]
    public void UpdateAddress_ShouldUpdateAddress()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg",
            new DateTime(1980, 1, 1));
        var property = RealState.Test.Domain.Property.Property.Create("Test Property", "123 Main St", 100000m,
            "INT-001", 2020, owner.IdOwner);
        var newAddress = "456 Elm St";

        // Act
        property.UpdateInfo(property.Name, newAddress, property.CodeInternal, property.Year);

        // Assert
        Assert.That(property.Address, Is.EqualTo(newAddress));
    }

    [Test]
    public void AddImage_ShouldAddImageToPropertyImages()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg", new DateTime(1980, 1, 1));
        var property = RealState.Test.Domain.Property.Property.Create("Test Property", "123 Main St", 100000m, "INT-001", 2020, owner.IdOwner);
        var fileName = "image1.jpg";

        // Act
        property.AddImage(fileName);

        // Assert
        Assert.That(property.PropertyImages.Count, Is.EqualTo(1));
        var image = property.PropertyImages.First();
        Assert.That(image.File, Is.EqualTo(fileName));
        Assert.That(image.Enabled, Is.True);
    }

    [Test]
    public void ChangePrice_ShouldThrowException_WhenNegativePrice()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg", new DateTime(1980, 1, 1));
        var property = RealState.Test.Domain.Property.Property.Create("Test Property", "123 Main St", 100000m, "INT-001", 2020, owner.IdOwner);
        var negativePrice = -100m;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => property.ChangePrice(negativePrice));
    }

    [Test]
    public void UpdateInfo_ShouldUpdateAllFields()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg", new DateTime(1980, 1, 1));
        var property = RealState.Test.Domain.Property.Property.Create("Test Property", "123 Main St", 100000m, "INT-001", 2020, owner.IdOwner);
        var newName = "Updated Property";
        var newAddress = "789 Oak Ave";
        var newCodeInternal = "INT-002";
        var newYear = 2025;

        // Act
        property.UpdateInfo(newName, newAddress, newCodeInternal, newYear);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(property.Name, Is.EqualTo(newName));
            Assert.That(property.Address, Is.EqualTo(newAddress));
            Assert.That(property.CodeInternal, Is.EqualTo(newCodeInternal));
            Assert.That(property.Year, Is.EqualTo(newYear));
        });
    }

    [Test]
    public void AddTrace_ShouldAddTraceToPropertyTraces()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg", new DateTime(1980, 1, 1));
        var property = RealState.Test.Domain.Property.Property.Create("Test Property", "123 Main St", 100000m, "INT-001", 2020, owner.IdOwner);
        var dateSale = new DateTime(2025, 9, 3);
        var traceName = "Sale Trace";
        var value = 150000m;
        var tax = 5000m;

        // Act
        property.AddTrace(dateSale, traceName, value, tax);

        // Assert
        Assert.That(property.PropertyTraces.Count, Is.EqualTo(1));
        var trace = property.PropertyTraces.First();
        Assert.That(trace.DateSale, Is.EqualTo(dateSale));
        Assert.That(trace.Name, Is.EqualTo(traceName));
        Assert.That(trace.Value, Is.EqualTo(value));
        Assert.That(trace.Tax, Is.EqualTo(tax));
    }
}