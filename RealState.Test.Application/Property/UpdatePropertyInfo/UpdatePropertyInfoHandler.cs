using FluentValidation;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.UpdatePropertyInfo;

public interface IUpdatePropertyInfoHandler
{
    Task HandleAsync(UpdatePropertyInfoCommand command, CancellationToken cancellationToken = default);
}

public class UpdatePropertyInfoHandler : IUpdatePropertyInfoHandler
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IValidator<UpdatePropertyInfoCommand> _validator;

    public UpdatePropertyInfoHandler(IPropertyRepository propertyRepository,
        IValidator<UpdatePropertyInfoCommand> validator)
    {
        _propertyRepository = propertyRepository;
        _validator = validator;
    }

    public async Task HandleAsync(UpdatePropertyInfoCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var property = await _propertyRepository.GetById(command.IdProperty, cancellationToken);

        if (property is null)
        {
            throw new InvalidOperationException("Property not found");
        }

        property.UpdateInfo(command.Name, command.Address, command.CodeInternal, command.Year);

        await _propertyRepository.SaveAsync(cancellationToken);
    }
}