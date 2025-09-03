namespace RealState.Test.Application.Property.AddImage;

public record AddImageCommand(Guid IdProperty, string FileName, Stream Stream);