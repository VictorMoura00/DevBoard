using DevBoard.Modules.Projects.Models;

namespace DevBoard.Modules.Projects.Data;

public interface IProjectRepository
{
    Task<IReadOnlyCollection<Project>> ListAsync(CancellationToken cancellationToken = default);

    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default);

    Task<Project?> UpdateAsync(Project project, CancellationToken cancellationToken = default);
}
