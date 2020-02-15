using CrossHMI.LibraryIntegration.Interfaces;

namespace CrossHMI.LibraryIntegration
{
    /// <summary>
    /// Delegate containing data with variable update.
    /// </summary>
    /// <typeparam name="T">The type of updated variable.</typeparam>
    /// <param name="deviceUpdateSourceBase">Originating source.</param>
    /// <param name="value">The new value of the variable.</param>
    public delegate void NetworkVariableUpdateEventHandler<T>(
        INetworkVariableUpdateSource<T> deviceUpdateSourceBase,
        T value);
}