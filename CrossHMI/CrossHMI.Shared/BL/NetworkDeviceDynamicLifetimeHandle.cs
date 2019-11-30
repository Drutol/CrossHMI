using System;
using CrossHMI.Interfaces.Networking;

namespace CrossHMI.Shared.BL
{
    public class NetworkDeviceDynamicLifetimeHandle : INetworkDeviceDynamicLifetimeHandle
    {
        private readonly NetworkEventsManager _networkEventsManager;

        public NetworkDeviceDynamicLifetimeHandle(NetworkEventsManager networkEventsManager)
        {
            _networkEventsManager = networkEventsManager;
        }

        public INetworkDeviceUpdateSourceBase DeviceUpdateSourceBase { get; set; }

        public void NotifyNewBindingCreated(
            string repository,
            string processValue,
            Type bindingType)
        {

            var method = typeof(NetworkDeviceDynamicLifetimeHandle)
                .GetMethod(nameof(RegisterVariable))?
                .MakeGenericMethod(bindingType);

            method.Invoke(this, new object[] { repository, processValue });
        }

        public void RegisterVariable<T>(
            string repository,
            string processValue)
        {
            DeviceUpdateSourceBase.RegisterNetworkVariable(
                _networkEventsManager.ObtainEventSourceForVariable<T>(repository, processValue));
        }
    }
}