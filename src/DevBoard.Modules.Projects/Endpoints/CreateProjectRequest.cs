namespace DevBoard.Modules.Projects.Endpoints;

public sealed record CreateProjectRequest(
    string Name,
    string? Description,
    string? RepositoryUrl,
    IReadOnlyCollection<string>? Tags);
