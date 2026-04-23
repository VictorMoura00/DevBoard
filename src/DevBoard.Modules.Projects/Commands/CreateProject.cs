namespace DevBoard.Modules.Projects.Commands;

public sealed record CreateProject(
    string Name,
    string? Description,
    string? RepositoryUrl,
    IReadOnlyCollection<string> Tags);
