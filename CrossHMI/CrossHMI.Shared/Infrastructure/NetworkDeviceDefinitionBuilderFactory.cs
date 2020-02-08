using Autofac;
using CrossHMI.LibraryIntegration.Infrastructure;
using CrossHMI.LibraryIntegration.Interfaces;

namespace CrossHMI.Shared.Infrastructure
{
    internal class NetworkDeviceDefinitionBuilderFactory : INetworkDeviceDefinitionBuilderFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public NetworkDeviceDefinitionBuilderFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public INetworkDeviceDefinitionBuilder CreateBuilder<T>(INetworkDeviceUpdateSourceBase source) where T : INetworkDevice
        {
            return _lifetimeScope.Resolve<NetworkDeviceDefinitionBuilder<T>>().WithUpdateSource(source);
        }
    }
}
