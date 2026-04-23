using DevBoard.Infrastructure;

namespace DevBoard.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddProblemDetails();
        services.AddOpenApi();
        services.AddControllers();
        services.AddHealthChecks();

        services
            .AddDevBoardInfrastructure(configuration)
            .AddDiscoveredApiModules(configuration);

        return services;
    }
}
