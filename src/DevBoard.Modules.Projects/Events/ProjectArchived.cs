using DevBoard.SharedKernel.Messaging;

namespace DevBoard.Modules.Projects.Events;

public sealed record ProjectArchived(
    Guid ProjectId,
    string ProjectName,
    DateTimeOffset OccurredAt) : IDomainEvent;
