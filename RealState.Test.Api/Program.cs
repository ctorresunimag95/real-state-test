using RealState.Test.Api.Common.Versioning;
using RealState.Test.Api.Common.Errors;
using RealState.Test.Api.Endpoints.Property;
using RealState.Test.Application;
using RealState.Test.Infrastructure;
using RealState.Test.Infrastructure.Persistence.Seed;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCustomApiVersioning();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetailsHandling();

var app = builder.Build();

app.CreateVersionSet();

app.UseStatusCodePages();
app.UseExceptionHandler();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapPropertyEndpoints();

await app.SeedDatabase();

app.Run();