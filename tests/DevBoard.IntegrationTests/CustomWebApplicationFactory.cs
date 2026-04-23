using DevBoard.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace DevBoard.IntegrationTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string DatabaseConnectionStringKey = "ConnectionStrings__Database";

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("devboard_test_db")
        .WithUsername("devboard")
        .WithPassword("devboard")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Database"] = _postgres.GetConnectionString()
            };

            configurationBuilder.AddInMemoryCollection(settings);
        });
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        Environment.SetEnvironmentVariable(DatabaseConnectionStringKey, _postgres.GetConnectionString());
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Environment.SetEnvironmentVariable(DatabaseConnectionStringKey, null);
        await _postgres.DisposeAsync().AsTask();
        Dispose();
    }

    public async Task<int> CountPendingMigrationsAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DevBoardDbContext>();

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        return pendingMigrations.Count();
    }

    public async Task ResetDatabaseAsync()
    {
        using var client = CreateClient();
        using var response = await client.GetAsync("/health");
        response.EnsureSuccessStatusCode();

        await using var connection = new NpgsqlConnection(_postgres.GetConnectionString());
        await connection.OpenAsync();

        const string sql = """
            TRUNCATE TABLE
                projects.projects,
                tasks.tasks,
                notifications.notifications
            RESTART IDENTITY CASCADE;
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
