using System;
using System.Threading;
using System.Threading.Tasks;
using CrossHMI.AzureGatewayService.Devices;
using CrossHMI.AzureGatewayService.Interfaces;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossHMI.AzureGatewayService.Infrastructure
{
    public class Bootstrapper : BackgroundService, IBootstrapper
    {
        private readonly IAzurePublisher _azurePublisher;
        private readonly INetworkDeviceDefinitionBuilderFactory _builderFactory;
        private readonly ILogger<Bootstrapper> _logger;
        private readonly INetworkEventsManager _networkEventsManager;
        private readonly IServiceProvider _serviceProvider;

        public Bootstrapper(
            ILogger<Bootstrapper> logger,
            ILibraryLogger libraryLogger,
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

            Task.Factory.StartNew(RunAzurePublisher);
        }

        private async void RunAzurePublisher()
        {
            await _azurePublisher.StartAsync(CancellationToken.None);
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

            await _azurePublisher.RegisterDeviceForPublishingAsync(boilerSource1.Device).ConfigureAwait(false);
            //await _azurePublisher.RegisterDeviceForPublishingAsync(boilerSource2.Device);
        }
    }
}