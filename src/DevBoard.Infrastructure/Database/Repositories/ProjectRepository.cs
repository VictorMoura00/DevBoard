using DevBoard.Infrastructure.Database;
using DevBoard.Modules.Projects.Data;
using DevBoard.Modules.Projects.Models;
using Microsoft.EntityFrameworkCore;

namespace DevBoard.Infrastructure.Database.Repositories;

public sealed class ProjectRepository(DevBoardDbContext dbContext) : IProjectRepository
{
    public async Task<IReadOnlyCollection<Project>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .AsNoTracking()
            .OrderByDescending(project => project.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(project);

        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        return project;
    }

    public async Task<Project?> UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(project);

        dbContext.Projects.Update(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        return project;
    }
}
