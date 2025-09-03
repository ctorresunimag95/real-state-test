using FluentValidation;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.Create;

public interface ICreatePropertyHandler
{
    Task<Guid> HandleAsync(CreatePropertyCommand command, CancellationToken cancellationToken = default);
}

public class CreatePropertyHandler : ICreatePropertyHandler
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IValidator<CreatePropertyCommand> _validator;

    public CreatePropertyHandler(IPropertyRepository propertyRepository, IValidator<CreatePropertyCommand> validator)
    {
        _propertyRepository = propertyRepository;
        _validator = validator;
    }

    public async Task<Guid> HandleAsync(CreatePropertyCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var property = Domain.Property.Property.Create(command.Name
            , command.Address
            , command.Price
            , command.CodeInternal
            , command.Year
            , command.IdOwner);

        await _propertyRepository.AddAsync(property, cancellationToken);

        await _propertyRepository.SaveAsync(cancellationToken);

        return property.IdProperty;
    }
}