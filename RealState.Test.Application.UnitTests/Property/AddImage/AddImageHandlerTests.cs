using FluentValidation;
using FluentValidation.Results;
using Moq;
using RealState.Test.Application.Property.AddImage;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.UnitTests.Property.AddImage;

[TestFixture]
public class AddImageHandlerTests
{
    private Mock<IImageStoreProvider> _imageStoreProviderMock;
    private Mock<IPropertyRepository> _propertyRepositoryMock;
    private Mock<IValidator<AddImageCommand>> _validatorMock;

    [SetUp]
    public void SetUp()
    {
        _imageStoreProviderMock = new Mock<IImageStoreProvider>();
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _validatorMock = new Mock<IValidator<AddImageCommand>>();
    }

    [Test]
    public void HandleAsync_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new AddImageCommand(
            IdProperty: Guid.NewGuid(),
            FileName: "image.jpg",
            Stream: new MemoryStream()
        );
        var validationResult = new ValidationResult([new ValidationFailure("FileName", "FileName is required")]);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        var handler = new AddImageHandler(_imageStoreProviderMock.Object, _propertyRepositoryMock.Object,
            _validatorMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await handler.HandleAsync(command));
        Assert.That(ex.Errors, Is.Not.Empty);
    }

    [Test]
    public void HandleAsync_ShouldThrowInvalidOperationException_WhenPropertyNotFound()
    {
        // Arrange
        var command = new AddImageCommand(
            IdProperty: Guid.NewGuid(),
            FileName: "image.jpg",
            Stream: new MemoryStream()
        );
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _propertyRepositoryMock.Setup(r => r.GetById(command.IdProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RealState.Test.Domain.Property.Property)null);
        var handler = new AddImageHandler(_imageStoreProviderMock.Object, _propertyRepositoryMock.Object,
            _validatorMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.HandleAsync(command));
        Assert.That(ex.Message, Is.EqualTo("Property not found"));
    }

    [Test]
    public async Task HandleAsync_ShouldAddImageAndReturnUrl_WhenCommandIsValid()
    {
        // Arrange
        var command = new AddImageCommand(
            IdProperty: Guid.NewGuid(),
            FileName: "image.jpg",
            Stream: new MemoryStream()
        );
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        var property = RealState.Test.Domain.Property.Property.Create(
            name: "Test Property",
            address: "123 Main St",
            price: 100000m,
            codeInternal: "INT-001",
            year: 2020,
            idOwner: Guid.NewGuid()
        );
        _propertyRepositoryMock.Setup(r => r.GetById(command.IdProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);
        var imageUrl = "https://images.com/image.jpg";
        _imageStoreProviderMock
            .Setup(p => p.SaveAsync(command.IdProperty, command.FileName, command.Stream,
                It.IsAny<CancellationToken>())).ReturnsAsync(imageUrl);
        _propertyRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new AddImageHandler(_imageStoreProviderMock.Object, _propertyRepositoryMock.Object,
            _validatorMock.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.That(result, Is.EqualTo(imageUrl));
        Assert.That(property.PropertyImages.Count, Is.EqualTo(1));
        Assert.That(property.PropertyImages.First().File, Is.EqualTo(imageUrl));
        _propertyRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}