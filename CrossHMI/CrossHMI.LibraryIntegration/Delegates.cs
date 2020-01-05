using CrossHMI.LibraryIntegration.Interfaces;

namespace CrossHMI.LibraryIntegration
{
    public delegate void NetworkVariableUpdateEventHandler<T>(
        INetworkVariableUpdateSource<T> deviceUpdateSourceBase,
        T value);
}