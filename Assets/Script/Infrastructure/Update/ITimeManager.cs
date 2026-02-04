namespace Infrastructure
{
    public interface ITimeManager
    {
        float DeltaTime { get; }
        float FixedDeltaTime { get; }
        float UnscaledDeltaTime { get; }
        float CurrentTime { get; }
        float UnscaledTime { get; }
    }
}