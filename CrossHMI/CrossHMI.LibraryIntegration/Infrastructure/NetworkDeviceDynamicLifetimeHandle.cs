using System;
using CrossHMI.LibraryIntegration.Interfaces;

namespace CrossHMI.LibraryIntegration.Infrastructure
{
    public class NetworkDeviceDynamicLifetimeHandle : INetworkDeviceDynamicLifetimeHandle
    {
        private readonly INetworkEventsManager _networkEventsManager;

        public NetworkDeviceDynamicLifetimeHandle(INetworkEventsManager networkEventsManager)
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