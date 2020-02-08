using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrossHMI.AzureGatewayService.Devices;
using CrossHMI.AzureGatewayService.Interfaces;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossHMI.AzureGatewayService.Infrastructure
{
    public class Bootstrapper : BackgroundService, IBootstrapper
    {
        private readonly ILogger<Bootstrapper> _logger;
        private readonly IAzurePublisher _azurePublisher;
        private readonly INetworkEventsManager _networkEventsManager;
        private readonly INetworkDeviceDefinitionBuilderFactory _builderFactory;
        private readonly IServiceProvider _serviceProvider;

        public Bootstrapper(
            ILogger<Bootstrapper> logger,
            IAzurePublisher azurePublisher,
            INetworkEventsManager networkEventsManager,
            INetworkDeviceDefinitionBuilderFactory builderFactory,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _azurePublisher = azurePublisher;
            _networkEventsManager = networkEventsManager;
            _builderFactory = builderFactory;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _networkEventsManager.Initialize().ConfigureAwait(false);

            var boilerSource1 =
                _networkEventsManager.ObtainEventSourceForDevice(
                    "BoilersArea_Boiler #1",
                    _serviceProvider.GetService<Boiler>);
            var boilerSource2 =
                _networkEventsManager.ObtainEventSourceForDevice(
                    "BoilersArea_Boiler #2",
                    _serviceProvider.GetService<Boiler>);

            _azurePublisher.RegisterDeviceForPublishing(boilerSource1.Device);
            _azurePublisher.RegisterDeviceForPublishing(boilerSource2.Device);
        }
    }
}
