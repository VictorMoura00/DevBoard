using DevBoard.Infrastructure.Database;
using DevBoard.SharedKernel.Time;
using Microsoft.EntityFrameworkCore;
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

        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' was not found.");

        services.AddDbContext<DevBoardDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "infrastructure");
            }));

        services.AddHealthChecks()
            .AddNpgSql(connectionString, name: "postgres");

        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}
