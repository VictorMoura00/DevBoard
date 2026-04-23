using DevBoard.Modules.Projects.Commands;
using DevBoard.Modules.Projects.Data;
using DevBoard.Modules.Projects.Models;
using DevBoard.SharedKernel.Time;

namespace DevBoard.Modules.Projects.Handlers;

public sealed class CreateProjectHandler(
    IProjectRepository projectRepository,
    ISystemClock clock)
{
    public async Task<Project> HandleAsync(CreateProject command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = command.Name.Trim(),
            Description = command.Description?.Trim(),
            RepositoryUrl = command.RepositoryUrl?.Trim(),
            Tags = NormalizeTags(command.Tags),
            Status = ProjectStatus.Active,
            CreatedAt = clock.UtcNow
        };

        return await projectRepository.AddAsync(project, cancellationToken);
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
