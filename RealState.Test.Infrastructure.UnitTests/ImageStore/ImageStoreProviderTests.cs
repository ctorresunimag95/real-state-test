using NUnit.Framework;
using RealState.Test.Infrastructure.ImageStore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RealState.Test.Infrastructure.UnitTests.ImageStore;

[TestFixture]
public class ImageStoreProviderTests
{
    [Test]
    public async Task SaveAsync_ShouldSaveFileAndReturnRelativePath()
    {
        // Arrange
        var provider = new ImageStoreProvider();
        var idProperty = Guid.NewGuid();
        var fileName = "test-image.png";
        var testContent = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(testContent);
        var cancellationToken = CancellationToken.None;

        // Act
        var relativePath = await provider.SaveAsync(idProperty, fileName, stream, cancellationToken);

        // Assert
        Assert.That(relativePath, Does.StartWith("Images/" + idProperty));
        Assert.That(relativePath, Does.EndWith(".png"));
        // Check file exists on disk
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        Assert.That(File.Exists(fullPath), Is.True);
        // Clean up
        File.Delete(fullPath);
        Directory.Delete(Path.GetDirectoryName(fullPath)!);
    }
}
