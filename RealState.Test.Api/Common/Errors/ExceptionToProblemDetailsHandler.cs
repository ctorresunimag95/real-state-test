using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;

namespace RealState.Test.Api.Common.Errors;

public class ExceptionToProblemDetailsHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public ExceptionToProblemDetailsHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var (errorMessage, statusCode) = exception switch
        {
            ValidationException validationResult => (validationResult.Message, StatusCodes.Status400BadRequest),
            InvalidOperationException invalidOperationException => (invalidOperationException.Message, StatusCodes.Status400BadRequest),
            _ => (exception.Message, StatusCodes.Status500InternalServerError),
        };
        
        httpContext.Response.StatusCode = statusCode;
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = "An error occurred while processing your request.",
                Detail = errorMessage,
                Type = exception.GetType().Name,
            },
            Exception = exception
        });
    }
}