using Microsoft.AspNetCore.Http;
using Moq;
using RealState.Test.Api.Endpoints.Property;
using RealState.Test.Api.Endpoints.Property.AddImage;
using RealState.Test.Api.Endpoints.Property.ChangePrice;
using RealState.Test.Api.Endpoints.Property.CreateProperty;
using RealState.Test.Api.Endpoints.Property.ListProperties;
using RealState.Test.Api.Endpoints.Property.UpdatePropertyInfo;
using RealState.Test.Application.Property.AddImage;
using RealState.Test.Application.Property.ChangePrice;
using RealState.Test.Application.Property.Create;
using RealState.Test.Application.Property.ListProperties;
using RealState.Test.Application.Property.UpdatePropertyInfo;
using RealState.Test.Domain.Property;

namespace RealState.Test.Api.Tests.Endpoints.Property;

[TestFixture]
public class EndpointTests
{
    [Test]
    public async Task CreateProperty_ShouldReturnCreated()
    {
        // Arrange
        var handlerMock = new Mock<ICreatePropertyHandler>();
        var request =
            new CreatePropertyRequest("Test Property", "123 Main St", 100000m, "INT-001", 2020, Guid.NewGuid());
        var propertyId = Guid.NewGuid();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<CreatePropertyCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(propertyId);

        // Act
        var result = await PropertyEndpoints.CreateProperty(request, handlerMock.Object);

        // Assert
        Assert.That(result.Result, Is.TypeOf<Microsoft.AspNetCore.Http.HttpResults.Created<CreatePropertyResponse>>());
        var created = result.Result as Microsoft.AspNetCore.Http.HttpResults.Created<CreatePropertyResponse>;
        Assert.That(created!.Value!.IdProperty, Is.EqualTo(propertyId));
    }

    [Test]
    public async Task ChangePrice_ShouldReturnNoContent()
    {
        // Arrange
        var handlerMock = new Mock<IChangePropertyPriceHandler>();
        var idProperty = Guid.NewGuid();
        var request = new ChangePropertyPriceRequest(120000m);
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<ChangePropertyPriceCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await PropertyEndpoints.ChangePrice(idProperty, request, handlerMock.Object);

        // Assert
        Assert.That(result.Result, Is.TypeOf<Microsoft.AspNetCore.Http.HttpResults.NoContent>());
    }

    [Test]
    public async Task UpdatePropertyInfo_ShouldReturnNoContent()
    {
        // Arrange
        var handlerMock = new Mock<IUpdatePropertyInfoHandler>();
        var idProperty = Guid.NewGuid();
        var request = new UpdatePropertyInfoRequest("Updated Property", "456 Elm St", "INT-002", 2025);
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<UpdatePropertyInfoCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await PropertyEndpoints.UpdatePropertyInfo(idProperty, request, handlerMock.Object);

        // Assert
        Assert.That(result.Result, Is.TypeOf<Microsoft.AspNetCore.Http.HttpResults.NoContent>());
    }

    [Test]
    public async Task AddImage_ShouldReturnOk()
    {
        // Arrange
        var handlerMock = new Mock<IAddImageHandler>();
        var idProperty = Guid.NewGuid();
        var fileMock = new Mock<IFormFile>();
        var fileName = "image.jpg";
        var stream = new MemoryStream();
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<AddImageCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://images.com/image.jpg");

        // Act
        var result = await PropertyEndpoints.AddImage(idProperty, fileMock.Object, handlerMock.Object);

        // Assert
        Assert.That(result.Result, Is.TypeOf<Microsoft.AspNetCore.Http.HttpResults.Ok<AddPropertyImageResponse>>());
        var ok = result.Result as Microsoft.AspNetCore.Http.HttpResults.Ok<AddPropertyImageResponse>;
        Assert.That(ok!.Value!.IdProperty, Is.EqualTo(idProperty));
        Assert.That(ok.Value.Url, Is.EqualTo("https://images.com/image.jpg"));
    }

    [Test]
    public async Task GetProperties_ShouldReturnOkWithMappedResults()
    {
        // Arrange
        var handlerMock = new Mock<IListPropertiesHandler>();
        var filters = new PropertyFilters("Test Property", "123 Main St", "INT-001", 100000m, 2020, Guid.NewGuid());
        
        var propertyResponse = new GetPropertiesResponse(Guid.NewGuid(), "Test Property", "123 Main St", 100000m,
            "INT-001",
            2020,
            new RealState.Test.Application.Property.ListProperties.PropertyOwnerResponse(filters.IdOwner!.Value,
                "John Doe", "Owner Address"),
            new List<RealState.Test.Application.Property.ListProperties.PropertyImageResponse>
            {
                new(Guid.NewGuid(), "https://images.com/image.jpg", true)
            });
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<GetPropertiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GetPropertiesResponse>
                { propertyResponse });

        // Act
        var result = await PropertyEndpoints.GetProperties(filters, handlerMock.Object);

        // Assert
        Assert.That(result.Result,
            Is.TypeOf<Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<PropertyResponse>>>());
        var ok = result.Result as Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<PropertyResponse>>;
        Assert.That(ok.Value.Count(), Is.EqualTo(1));
        Assert.That(ok.Value.First().Name, Is.EqualTo("Test Property"));
    }
}