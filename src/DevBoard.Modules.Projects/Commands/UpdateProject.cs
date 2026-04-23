using DevBoard.Modules.Projects.Models;

namespace DevBoard.Modules.Projects.Commands;

public sealed record UpdateProject(
    Guid Id,
    string Name,
    string? Description,
    string? RepositoryUrl,
    ProjectStatus Status,
    IReadOnlyCollection<string> Tags);
