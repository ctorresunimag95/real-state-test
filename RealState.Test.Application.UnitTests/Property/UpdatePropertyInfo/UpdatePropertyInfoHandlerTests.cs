using FluentValidation;
using FluentValidation.Results;
using Moq;
using RealState.Test.Application.Property.UpdatePropertyInfo;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.UnitTests.Property.UpdatePropertyInfo;

[TestFixture]
public class UpdatePropertyInfoHandlerTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock;
    private Mock<IValidator<UpdatePropertyInfoCommand>> _validatorMock;

    [SetUp]
    public void SetUp()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _validatorMock = new Mock<IValidator<UpdatePropertyInfoCommand>>();
    }

    [Test]
    public void HandleAsync_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new UpdatePropertyInfoCommand(
            IdProperty: Guid.NewGuid(),
            Name: "",
            Address: "",
            CodeInternal: "",
            Year: 1700
        );
        var validationResult = new ValidationResult([new ValidationFailure("Name", "Name is required")]);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        var handler = new UpdatePropertyInfoHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await handler.HandleAsync(command));
        Assert.That(ex.Errors, Is.Not.Empty);
    }

    [Test]
    public void HandleAsync_ShouldThrowInvalidOperationException_WhenPropertyNotFound()
    {
        // Arrange
        var command = new UpdatePropertyInfoCommand(
            IdProperty: Guid.NewGuid(),
            Name: "Test Property",
            Address: "123 Main St",
            CodeInternal: "INT-001",
            Year: 2020
        );
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _propertyRepositoryMock.Setup(r => r.GetById(command.IdProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RealState.Test.Domain.Property.Property)null);
        var handler = new UpdatePropertyInfoHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.HandleAsync(command));
        Assert.That(ex.Message, Is.EqualTo("Property not found"));
    }

    [Test]
    public async Task HandleAsync_ShouldUpdatePropertyAndSave_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdatePropertyInfoCommand(
            IdProperty: Guid.NewGuid(),
            Name: "Updated Property",
            Address: "456 Elm St",
            CodeInternal: "INT-002",
            Year: 2025
        );
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        var property = RealState.Test.Domain.Property.Property.Create(
            name: command.Name,
            address: command.Address,
            price: 100000m,
            codeInternal: command.CodeInternal,
            year: command.Year,
            idOwner: Guid.NewGuid()
        );
        _propertyRepositoryMock.Setup(r => r.GetById(command.IdProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);
        _propertyRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new UpdatePropertyInfoHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.That(property.Name, Is.EqualTo(command.Name));
        Assert.That(property.Address, Is.EqualTo(command.Address));
        Assert.That(property.CodeInternal, Is.EqualTo(command.CodeInternal));
        Assert.That(property.Year, Is.EqualTo(command.Year));
        _propertyRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}