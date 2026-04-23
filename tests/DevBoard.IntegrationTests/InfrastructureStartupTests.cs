namespace DevBoard.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public sealed class InfrastructureStartupTests(CustomWebApplicationFactory factory)
{
    [Fact]
    public async Task HealthEndpoint_ReturnsSuccess_WhenDatabaseIsAvailable()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/health");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Startup_AppliesAllMigrations()
    {
        using var client = factory.CreateClient();

        _ = await client.GetAsync("/health");

        var pendingMigrations = await factory.CountPendingMigrationsAsync();

        Assert.Equal(0, pendingMigrations);
    }

    [Fact]
    public async Task DocumentationEndpoint_ReturnsSuccess_InDevelopment()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/docs");

        response.EnsureSuccessStatusCode();
    }
}
