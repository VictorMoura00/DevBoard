using DevBoard.SharedKernel.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevBoard.Infrastructure;

public static class DevBoardInfrastructure
{
    public static IServiceCollection AddDevBoardInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}
