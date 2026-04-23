using DevBoard.Modules.Tasks.Models;

namespace DevBoard.Modules.Tasks.Commands;

public sealed record CreateTask(
    Guid ProjectId,
    string Title,
    string? Description,
    ProjectTaskPriority Priority,
    DateOnly? DueDate);
