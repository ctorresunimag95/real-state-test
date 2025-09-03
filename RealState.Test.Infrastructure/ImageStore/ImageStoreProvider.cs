using RealState.Test.Domain.Property;

namespace RealState.Test.Infrastructure.ImageStore;

public class ImageStoreProvider : IImageStoreProvider
{
    private static readonly string BaseImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

    public async Task<string> SaveAsync(Guid idProperty, string fileName, Stream stream,
        CancellationToken cancellationToken = default)
    {
        // Create directory for this property if it doesn't exist
        string propertyDirectory = Path.Combine(BaseImagePath, idProperty.ToString());
        Directory.CreateDirectory(propertyDirectory);

        // Generate a unique filename to avoid collisions
        string extension = Path.GetExtension(fileName);
        string uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.UtcNow.Ticks}{extension}";
        string fullPath = Path.Combine(propertyDirectory, uniqueFileName);

        // Create relative path for database storage
        string relativePath = Path.Combine("Images", idProperty.ToString(), uniqueFileName);

        // Save the file
        await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None,
            bufferSize: 4096, useAsync: true);

        await stream.CopyToAsync(fileStream, cancellationToken);

        return relativePath.Replace("\\", "/");
    }
}