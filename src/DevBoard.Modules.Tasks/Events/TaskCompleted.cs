using DevBoard.SharedKernel.Messaging;

namespace DevBoard.Modules.Tasks.Events;

public sealed record TaskCompleted(
    Guid TaskId,
    Guid ProjectId,
    string Title,
    DateTimeOffset OccurredAt) : IDomainEvent;
