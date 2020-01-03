using CrossHMI.Interfaces.Networking;

namespace CrossHMI.Shared.EventSources
{
    /// <inheritdoc />
    public class NetworkDeviceUpdateSource<T> : INetworkDeviceUpdateSource<T>
        where T : INetworkDevice
    {
        /// <inheritdoc />
        public T Device { get; set; }

        /// <inheritdoc />
        public void RegisterNetworkVariable<TProperty>(
            INetworkVariableUpdateSource<TProperty> networkVariableUpdateSource)
        {
            networkVariableUpdateSource.Updated += (updateSource, value) =>
            {
                Device?.ProcessPropertyUpdate(updateSource.Name, value);
            };
        }
    }
}