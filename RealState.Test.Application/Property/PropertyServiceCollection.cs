using Microsoft.Extensions.DependencyInjection;
using RealState.Test.Application.Property.AddImage;
using RealState.Test.Application.Property.ChangePrice;
using RealState.Test.Application.Property.Create;
using RealState.Test.Application.Property.ListProperties;
using RealState.Test.Application.Property.UpdatePropertyInfo;

namespace RealState.Test.Application.Property;

internal static class PropertyServiceCollection
{
    public static void AddProperty(this IServiceCollection services)
    {
        services.AddScoped<IChangePropertyPriceHandler, ChangePropertyPriceHandler>();
        services.AddScoped<ICreatePropertyHandler, CreatePropertyHandler>();
        services.AddScoped<IUpdatePropertyInfoHandler, UpdatePropertyInfoHandler>();
        services.AddScoped<IAddImageHandler, AddImageHandler>();
        services.AddScoped<IListPropertiesHandler, ListPropertiesHandler>();
    }
}