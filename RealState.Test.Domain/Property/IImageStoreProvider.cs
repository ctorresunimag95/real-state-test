namespace RealState.Test.Domain.Property;

public interface IImageStoreProvider
{
    Task<string> SaveAsync(Guid idProperty, string fileName, Stream stream, CancellationToken cancellationToken = default);
}