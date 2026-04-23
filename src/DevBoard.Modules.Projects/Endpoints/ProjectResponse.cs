using DevBoard.Modules.Projects.Models;

namespace DevBoard.Modules.Projects.Endpoints;

public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    string? RepositoryUrl,
    ProjectStatus Status,
    IReadOnlyCollection<string> Tags,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? ArchivedAt)
{
    public static ProjectResponse FromModel(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

        return new ProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.RepositoryUrl,
            project.Status,
            project.Tags,
            project.CreatedAt,
            project.UpdatedAt,
            project.ArchivedAt);
    }
}
