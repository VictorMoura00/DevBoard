using DevBoard.Modules.Tasks.Models;

namespace DevBoard.Modules.Tasks.Commands;

public sealed record UpdateTask(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    ProjectTaskStatus Status,
    ProjectTaskPriority Priority,
    DateOnly? DueDate);
