using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Interfaces;

namespace CrossHMI.Shared.Infrastructure
{
    internal class NetworkDeviceDefinitionBuilderFactory : INetworkDeviceDefinitionBuilderFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public NetworkDeviceDefinitionBuilderFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public NetworkDeviceDefinitionBuilder<T> CreateBuilder<T>(INetworkDeviceUpdateSourceBase source) where T : INetworkDevice
        {
            return _lifetimeScope.Resolve<NetworkDeviceDefinitionBuilder<T>>(
                new TypedParameter(typeof(INetworkDeviceUpdateSourceBase), source));
        }
    }
}
