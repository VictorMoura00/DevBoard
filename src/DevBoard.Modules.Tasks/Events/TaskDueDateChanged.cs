using DevBoard.SharedKernel.Messaging;

namespace DevBoard.Modules.Tasks.Events;

public sealed record TaskDueDateChanged(
    Guid TaskId,
    Guid ProjectId,
    DateOnly? PreviousDueDate,
    DateOnly? NewDueDate,
    DateTimeOffset OccurredAt) : IDomainEvent;
