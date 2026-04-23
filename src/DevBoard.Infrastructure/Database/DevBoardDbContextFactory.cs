using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DevBoard.Infrastructure.Database;

public sealed class DevBoardDbContextFactory : IDesignTimeDbContextFactory<DevBoardDbContext>
{
    public DevBoardDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? "Host=localhost;Port=5432;Database=devboard_db;Username=devboard;Password=devboard";

        var optionsBuilder = new DbContextOptionsBuilder<DevBoardDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "infrastructure");
        });

        return new DevBoardDbContext(optionsBuilder.Options);
    }
}
