using DevBoard.Modules.Projects.Models;

namespace DevBoard.Modules.Projects.Endpoints;

public sealed record UpdateProjectRequest(
    string Name,
    string? Description,
    string? RepositoryUrl,
    ProjectStatus Status,
    IReadOnlyCollection<string>? Tags);
