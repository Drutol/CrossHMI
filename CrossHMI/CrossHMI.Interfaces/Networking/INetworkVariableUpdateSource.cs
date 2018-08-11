using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    /// Interface of component meant to wrap <see cref="IConsumerBinding"/> exposing an event with update information.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INetworkVariableUpdateSource<T>
    {
        /// <summary>
        /// The name of the process variable that propagates updates.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Event for when the new value is received.
        /// </summary>
        event NetworkVariableUpdateEventHandler<T> Updated;
    }
}
