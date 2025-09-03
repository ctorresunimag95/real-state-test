using Asp.Versioning.Builder;

namespace RealState.Test.Api.Common.Versioning;

public static class Versioning
{
    public static ApiVersionSet VersionSet { get; private set; } = null!;

    public static IEndpointRouteBuilder CreateVersionSet(this IEndpointRouteBuilder app)
    {
        VersionSet = app.NewApiVersionSet()
            .HasApiVersion(new Asp.Versioning.ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        return app;
    }
}