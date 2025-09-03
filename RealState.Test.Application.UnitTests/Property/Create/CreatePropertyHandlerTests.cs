using FluentValidation;
using FluentValidation.Results;
using Moq;
using RealState.Test.Application.Property.Create;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.UnitTests.Property.Create;

[TestFixture]
public class CreatePropertyHandlerTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock;
    private Mock<IValidator<CreatePropertyCommand>> _validatorMock;

    [SetUp]
    public void SetUp()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _validatorMock = new Mock<IValidator<CreatePropertyCommand>>();
    }

    [Test]
    public void HandleAsync_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreatePropertyCommand(
            Name: "",
            Address: "",
            Price: 0,
            CodeInternal: "",
            Year: 1700,
            IdOwner: Guid.Empty
        );
        var validationResult = new ValidationResult([new ValidationFailure("Name", "Name is required")]);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        var handler = new CreatePropertyHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await handler.HandleAsync(command));
        Assert.That(ex.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task HandleAsync_ShouldAddPropertyAndReturnId_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreatePropertyCommand(
            Name: "Test Property",
            Address: "123 Main St",
            Price: 100000m,
            CodeInternal: "INT-001",
            Year: 2020,
            IdOwner: Guid.NewGuid()
        );
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _propertyRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<RealState.Test.Domain.Property.Property>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _propertyRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new CreatePropertyHandler(_propertyRepositoryMock.Object, _validatorMock.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        _propertyRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<RealState.Test.Domain.Property.Property>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _propertyRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}