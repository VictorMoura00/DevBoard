namespace DevBoard.SharedKernel.Time;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}
