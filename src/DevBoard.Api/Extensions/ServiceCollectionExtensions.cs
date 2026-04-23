using DevBoard.Infrastructure;
using DevBoard.Modules.Notifications;
using DevBoard.Modules.Projects;
using DevBoard.Modules.Tasks;

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
        services.AddHealthChecks();

        services
            .AddDevBoardInfrastructure(configuration)
            .AddProjectsModule()
            .AddTasksModule()
            .AddNotificationsModule();

        return services;
    }
}
