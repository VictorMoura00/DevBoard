using DevBoard.Modules.Projects.Commands;
using DevBoard.Modules.Projects.Data;
using DevBoard.Modules.Projects.Models;
using DevBoard.SharedKernel.Time;

namespace DevBoard.Modules.Projects.Handlers;

public sealed class ArchiveProjectHandler(
    IProjectRepository projectRepository,
    ISystemClock clock)
{
    public async Task<Project?> HandleAsync(ArchiveProject command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var project = await projectRepository.GetByIdAsync(command.Id, cancellationToken);

        if (project is null)
        {
            return null;
        }

        var now = clock.UtcNow;
        project.Status = ProjectStatus.Archived;
        project.ArchivedAt = now;
        project.UpdatedAt = now;

        return await projectRepository.UpdateAsync(project, cancellationToken);
    }
}
