namespace DevBoard.Modules.Projects.Models;

public sealed class Project
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? RepositoryUrl { get; set; }

    public ProjectStatus Status { get; set; } = ProjectStatus.Active;

    public List<string> Tags { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? ArchivedAt { get; set; }
}
