using DevBoard.Modules.Notifications.Models;
using DevBoard.Modules.Projects.Models;
using DevBoard.Modules.Tasks.Models;
using Microsoft.EntityFrameworkCore;

namespace DevBoard.Infrastructure.Database;

public sealed class DevBoardDbContext(DbContextOptions<DevBoardDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();

    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.HasDefaultSchema("public");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevBoardDbContext).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Project).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectTask).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Notification).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
