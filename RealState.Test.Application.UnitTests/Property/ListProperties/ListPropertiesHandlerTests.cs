using Moq;
using NUnit.Framework;
using RealState.Test.Application.Property.ListProperties;
using RealState.Test.Domain.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealState.Test.Application.UnitTests.Property.ListProperties;

[TestFixture]
public class ListPropertiesHandlerTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
    }

    [Test]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoPropertiesFound()
    {
        // Arrange
        var query = new GetPropertiesQuery();
        _propertyRepositoryMock.Setup(r => r.GetAsync(It.IsAny<PropertyFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RealState.Test.Domain.Property.Property>());
        var handler = new ListPropertiesHandler(_propertyRepositoryMock.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task HandleAsync_ShouldReturnMappedProperties_WhenPropertiesExist()
    {
        // Arrange
        var owner = Owner.Create("John Doe", "Owner Address", "photo.jpg", new DateTime(1980, 1, 1));
        var property = RealState.Test.Domain.Property.Property.Create(
            name: "Test Property",
            address: "123 Main St",
            price: 100000m,
            codeInternal: "INT-001",
            year: 2020,
            idOwner: owner.IdOwner
        );
        property.AddImage("https://images.com/image.jpg");
        var properties = new List<RealState.Test.Domain.Property.Property> { property };
        var query = new GetPropertiesQuery();
        _propertyRepositoryMock.Setup(r => r.GetAsync(It.IsAny<PropertyFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(properties);
        var handler = new ListPropertiesHandler(_propertyRepositoryMock.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        var response = result.First();
        Assert.That(response.IdProperty, Is.EqualTo(property.IdProperty));
        Assert.That(response.Name, Is.EqualTo(property.Name));
        Assert.That(response.Address, Is.EqualTo(property.Address));
        Assert.That(response.Price, Is.EqualTo(property.Price));
        Assert.That(response.CodeInternal, Is.EqualTo(property.CodeInternal));
        Assert.That(response.Year, Is.EqualTo(property.Year));
        Assert.That(response.Owner.IdOwner, Is.EqualTo(property.IdOwner));
        Assert.That(response.Owner.Name, Is.EqualTo(owner.Name));
        Assert.That(response.Owner.Address, Is.EqualTo(owner.Address));
        Assert.That(response.Images.Count(), Is.EqualTo(1));
        Assert.That(response.Images.First().Url, Is.EqualTo("https://images.com/image.jpg"));
    }

    [Test]
    public async Task HandleAsync_ShouldApplyFiltersCorrectly()
    {
        // Arrange
        var query = new GetPropertiesQuery(
            Name: "Filtered Property",
            Address: "Filtered Address",
            CodeInternal: "CODE-123",
            Price: 50000m,
            Year: 2022,
            IdOwner: Guid.NewGuid()
        );
        var handler = new ListPropertiesHandler(_propertyRepositoryMock.Object);

        // Act
        await handler.HandleAsync(query);

        // Assert
        _propertyRepositoryMock.Verify(r => r.GetAsync(
            It.Is<PropertyFilters>(f =>
                f.Name == query.Name &&
                f.Address == query.Address &&
                f.CodeInternal == query.CodeInternal &&
                f.Price == query.Price &&
                f.Year == query.Year &&
                f.IdOwner == query.IdOwner
            ), It.IsAny<CancellationToken>()), Times.Once);
    }
}
