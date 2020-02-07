using System;
using CrossHMI.LibraryIntegration.Infrastructure;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CrossHMI.AzureGatewayService.Infrastructure
{
    internal class NetworkDeviceDefinitionBuilderFactory : INetworkDeviceDefinitionBuilderFactory
    {
        private readonly IServiceProvider _lifetimeScope;

        public NetworkDeviceDefinitionBuilderFactory(IServiceProvider lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public INetworkDeviceDefinitionBuilder CreateBuilder<T>(INetworkDeviceUpdateSourceBase source) where T : INetworkDevice
        {
            var builder = _lifetimeScope.GetService<NetworkDeviceDefinitionBuilder<T>>();
            return builder.WithUpdateSource(source);
        }
    }
}
