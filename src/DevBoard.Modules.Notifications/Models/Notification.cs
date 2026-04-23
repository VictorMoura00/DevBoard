namespace DevBoard.Modules.Notifications.Models;

public sealed class Notification
{
    public Guid Id { get; set; }

    public required string Type { get; set; }

    public required string Title { get; set; }

    public required string Message { get; set; }

    public bool IsRead { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ReadAt { get; set; }

    public Guid? RelatedProjectId { get; set; }

    public Guid? RelatedTaskId { get; set; }
}
