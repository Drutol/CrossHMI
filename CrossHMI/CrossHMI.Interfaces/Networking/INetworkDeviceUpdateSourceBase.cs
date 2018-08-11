using System.ComponentModel;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    /// Non generic base of <see cref="INetworkDeviceUpdateSource{T}"/>.
    /// </summary>
    public interface INetworkDeviceUpdateSourceBase
    {
        /// <summary>
        /// Registers network variable and starts listening for updates.
        /// </summary>
        /// <typeparam name="TProperty">The type of property.</typeparam>
        /// <param name="networkVariableUpdateSource">The source of updates.</param>
        void RegisterNetworkVariable<TProperty>(INetworkVariableUpdateSource<TProperty> networkVariableUpdateSource);
    }
}
