namespace DevBoard.SharedKernel.Messaging;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
