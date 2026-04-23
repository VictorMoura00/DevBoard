using DevBoard.Modules.Projects.Commands;
using DevBoard.Modules.Projects.Data;
using DevBoard.Modules.Projects.Models;
using DevBoard.SharedKernel.Time;

namespace DevBoard.Modules.Projects.Handlers;

public sealed class UpdateProjectHandler(
    IProjectRepository projectRepository,
    ISystemClock clock)
{
    public async Task<Project?> HandleAsync(UpdateProject command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var project = await projectRepository.GetByIdAsync(command.Id, cancellationToken);

        if (project is null)
        {
            return null;
        }

        project.Name = command.Name.Trim();
        project.Description = command.Description?.Trim();
        project.RepositoryUrl = command.RepositoryUrl?.Trim();
        project.Status = command.Status;
        project.Tags = NormalizeTags(command.Tags);
        project.UpdatedAt = clock.UtcNow;

        if (project.Status is not ProjectStatus.Archived)
        {
            project.ArchivedAt = null;
        }

        return await projectRepository.UpdateAsync(project, cancellationToken);
    }

    private static string[] NormalizeTags(IReadOnlyCollection<string> tags)
    {
        return tags
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
