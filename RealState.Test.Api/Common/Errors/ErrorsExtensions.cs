namespace RealState.Test.Api.Common.Errors;

internal static class ErrorsExtensions
{
    public static IServiceCollection AddProblemDetailsHandling(this IServiceCollection services)
    {
        services
            .AddProblemDetails(options =>
                options.CustomizeProblemDetails = ctx =>
                {
                    ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
                    ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
                });
        services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();
        
        return services;
    }
}