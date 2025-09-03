using FluentValidation;
using RealState.Test.Domain.Property;

namespace RealState.Test.Application.Property.AddImage;

public interface IAddImageHandler
{
    Task<string> HandleAsync(AddImageCommand command, CancellationToken cancellationToken = default);
}

public class AddImageHandler : IAddImageHandler
{
    private readonly IImageStoreProvider _imageStoreProvider;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IValidator<AddImageCommand> _validator;

    public AddImageHandler(IImageStoreProvider imageStoreProvider, IPropertyRepository propertyRepository,
        IValidator<AddImageCommand> validator)
    {
        _imageStoreProvider = imageStoreProvider;
        _propertyRepository = propertyRepository;
        _validator = validator;
    }

    public async Task<string> HandleAsync(AddImageCommand command, CancellationToken cancellationToken = default)
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

        var imageUrl =
            await _imageStoreProvider.SaveAsync(command.IdProperty, command.FileName, command.Stream,
                cancellationToken);

        property.AddImage(imageUrl);

        await _propertyRepository.SaveAsync(cancellationToken);

        return imageUrl;
    }
}