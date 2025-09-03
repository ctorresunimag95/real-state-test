using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using RealState.Test.Application.Property.ChangePrice;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.UnitTests.Property.ChangePrice;

[TestFixture]
public class ChangePropertyPriceHandlerTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock;
    private Mock<IValidator<ChangePropertyPriceCommand>> _validatorMock;

    [SetUp]
    public void SetUp()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _validatorMock = new Mock<IValidator<ChangePropertyPriceCommand>>();
    }

    [Test]
    public void HandleAsync_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new ChangePropertyPriceCommand(
            IdProperty: Guid.NewGuid(),
            Price: -100m
        );
        var validationResult = new ValidationResult([new ValidationFailure("Price", "Price must be positive")]);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        var handler = new ChangePropertyPriceHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await handler.HandleAsync(command));
        Assert.That(ex.Errors, Is.Not.Empty);
    }

    [Test]
    public void HandleAsync_ShouldThrowInvalidOperationException_WhenPropertyNotFound()
    {
        // Arrange
        var command = new ChangePropertyPriceCommand(
            IdProperty: Guid.NewGuid(),
            Price: 100000m
        );
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _propertyRepositoryMock.Setup(r => r.GetById(command.IdProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RealState.Test.Domain.Property.Property)null);
        var handler = new ChangePropertyPriceHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.HandleAsync(command));
        Assert.That(ex.Message, Is.EqualTo("Property not found"));
    }

    [Test]
    public async Task HandleAsync_ShouldChangePriceAndSave_WhenCommandIsValid()
    {
        // Arrange
        var command = new ChangePropertyPriceCommand(
            IdProperty: Guid.NewGuid(),
            Price: 120000m
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
        _propertyRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new ChangePropertyPriceHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.That(property.Price, Is.EqualTo(command.Price));
        _propertyRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}