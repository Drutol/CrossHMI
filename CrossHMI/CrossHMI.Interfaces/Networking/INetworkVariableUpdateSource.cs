namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkVariableUpdateSource<T>
    {
        string Name { get; }

        event NetworkVariableUpdateEventHandler<T> Updated;
    }
}
