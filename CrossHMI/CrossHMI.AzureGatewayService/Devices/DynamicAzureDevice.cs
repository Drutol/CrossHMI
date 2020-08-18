using System;
using System.Collections.Generic;
using System.Text;
using CrossHMI.AzureGatewayService.Infrastructure.Configuration;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using CrossHMI.LibraryIntegration.Infrastructure.Devices;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CrossHMI.AzureGatewayService.Devices
{
    public class DynamicAzureDevice : NetworkDeviceBase, INetworkDynamicDevice, IAzureEnabledNetworkDevice
    {
        private readonly ILogger<DynamicAzureDevice> _logger;
        private string _repository;

        public INetworkDeviceDynamicLifetimeHandle Handle { get; private set; }

        public TimeSpan PublishingInterval { get; private set; }
        public IAzureDeviceParameters AzureDeviceParameters { get; private set; }

        private readonly Dictionary<string, string> _deviceState = new Dictionary<string, string>();

        public override string Repository
        {
            get => _repository;
            set
            {
                _repository = value;
                _logger.BeginScope($"Device:{value}");
            }
        }

        public DeviceClient DeviceClient { get; set; }

        public DynamicAzureDevice(ILogger<DynamicAzureDevice> logger)
        {
            _logger = logger;
        }

        public override void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            base.DefineDevice(builder);

            builder.RequestConfigurationExtenstion<BoilerRepositoryDetails>(data =>
            {
                AzureDeviceParameters = data;
                PublishingInterval = data.PublishingInterval;
            });

            Handle = builder.DeclareDynamic();
        }

        public override void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            _deviceState[variableName] = value.ToString();
        }

        public string CreateMessagePayload()
        {
            _logger.LogTrace("Building payload.");
            return JsonConvert.SerializeObject(_deviceState);
        }
    }
}
