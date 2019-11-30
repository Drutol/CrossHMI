namespace CrossHMI.Interfaces.Adapters
{
    public interface ILogAdapter<T>
    {
        void LogDebug(string message);
    }
}