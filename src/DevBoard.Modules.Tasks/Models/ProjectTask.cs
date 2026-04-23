namespace DevBoard.Modules.Tasks.Models;

public sealed class ProjectTask
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;

    public ProjectTaskPriority Priority { get; set; } = ProjectTaskPriority.Medium;

    public DateOnly? DueDate { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }
}
