using DevBoard.Api.Abstractions.Routing;

namespace DevBoard.Api.Extensions;

public static class ApiModuleExtensions
{
    public static IServiceCollection AddDiscoveredApiModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (var module in CreateModules())
        {
            module.AddServices(services, configuration);
            services.AddSingleton(typeof(IApiModule), module);
        }

        return services;
    }

    public static IEndpointRouteBuilder MapDiscoveredApiModules(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var modules = endpoints.ServiceProvider.GetServices<IApiModule>();

        foreach (var module in modules)
        {
            module.MapEndpoints(endpoints);
        }

        return endpoints;
    }

    private static IReadOnlyCollection<IApiModule> CreateModules()
    {
        return AssemblyExtensions.GetDevBoardAssemblies()
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           typeof(IApiModule).IsAssignableFrom(type))
            .Select(type => Activator.CreateInstance(type.AsType()))
            .OfType<IApiModule>()
            .OrderBy(module => module.GetType().FullName, StringComparer.Ordinal)
            .ToArray();
    }
}
