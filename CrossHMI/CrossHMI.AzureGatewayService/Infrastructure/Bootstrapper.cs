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
        private readonly Func<Boiler> _boilerFactory;
        private readonly Func<DynamicAzureDevice> _azureDeviceFactory;
        private readonly ILogger<Bootstrapper> _logger;
        private readonly INetworkEventsManager _networkEventsManager;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool _disposed;

        public Bootstrapper(
            ILogger<Bootstrapper> logger,
            ILibraryLogger libraryLogger,
            IAzurePublisher azurePublisher,
            INetworkEventsManager networkEventsManager,
            INetworkDeviceDefinitionBuilderFactory builderFactory,
            Func<Boiler> boilerFactory,
            Func<DynamicAzureDevice> azureDeviceFactory)
        {
            _logger = logger;
            _azurePublisher = azurePublisher;
            _networkEventsManager = networkEventsManager;
            _builderFactory = builderFactory;
            _boilerFactory = boilerFactory;
            _azureDeviceFactory = azureDeviceFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _networkEventsManager.NewDeviceCreated += NetworkEventsManagerOnNewDeviceCreated;
            _networkEventsManager.EnableAutomaticDeviceInstantiation(_azureDeviceFactory);
            await _networkEventsManager
                .Initialize()
                .ConfigureAwait(false);

            //var boilerSource1 =
            //    _networkEventsManager.ObtainEventSourceForDevice(
            //        "BoilersArea_Boiler #1",
            //        _boilerFactory);

            //var boilerSource2 =
            //    _networkEventsManager.ObtainEventSourceForDevice(
            //        "BoilersArea_Boiler #2",
            //        _serviceProvider.GetService<Boiler>);

            //await _azurePublisher.RegisterDeviceForPublishingAsync(boilerSource2.Device);
        }

        private async void NetworkEventsManagerOnNewDeviceCreated(object? sender, INetworkDeviceUpdateSource<INetworkDynamicDevice> e)
        {
            try
            {
                await _azurePublisher
                    .RegisterDeviceForPublishingAsync(e.Device as IAzureEnabledNetworkDevice)
                    .ConfigureAwait(false);
                _logger.LogInformation($"Registered device {e.Device.Repository} for publishing.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Failed to register device {e.Device.Repository} for publishing.");
            }
        }

        public override void Dispose()
        {
            if(_disposed)
                return;

            _disposed = true;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _logger.LogInformation("Cancelled AzurePublisher operation.");
            base.Dispose();
        }
    }
}