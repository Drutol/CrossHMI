using System;
using System.Text;
using System.Threading.Tasks;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using CrossHMI.LibraryIntegration.AzureGateway.Util;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Logging;

namespace CrossHMI.LibraryIntegration.AzureGateway.Infrastructure
{
    internal class AzurePublisherDeviceHandle : IDisposable
    {
        private const string GlobalDeviceEndpoint = "global.azure-devices-provisioning.net";

        private SecurityProviderSymmetricKey _security;
        private ProvisioningTransportHandler _transport;
        private DeviceClient _deviceClient;
        private readonly ILogger<AzurePublisherDeviceHandle> _logger;

        public IAzureEnabledNetworkDevice Device { get; }

        public AzurePublisherDeviceHandle(
            IAzureEnabledNetworkDevice device,
            ILoggerFactory loggerFactory)
        {
            device.AzureConnectionParameters.AssertNotNull();

            Device = device;

            _logger = loggerFactory?.CreateLogger<AzurePublisherDeviceHandle>();
            _logger?.BeginScope($"Device:{Device.AzureConnectionParameters.AzureDeviceId}");
        }

        public async Task<bool> Initialize()
        {
            _security = new SecurityProviderSymmetricKey(
                Device.AzureConnectionParameters.AzureDeviceId,
                Device.AzureConnectionParameters.AzurePrimaryKey,
                Device.AzureConnectionParameters.AzureSecondaryKey);

            switch (Device.AzureConnectionParameters.TransportType)
            {
                case TransportType.Amqp:
                    _transport = new ProvisioningTransportHandlerAmqp();
                    break;
                case TransportType.Http1:
                    _transport = new ProvisioningTransportHandlerHttp();
                    break;
                case TransportType.Amqp_WebSocket_Only:
                    _transport = new ProvisioningTransportHandlerAmqp(TransportFallbackType.WebSocketOnly);
                    break;
                case TransportType.Amqp_Tcp_Only:
                    _transport = new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly);
                    break;
                case TransportType.Mqtt:
                    _transport = new ProvisioningTransportHandlerMqtt();
                    break;
                case TransportType.Mqtt_WebSocket_Only:
                    _transport = new ProvisioningTransportHandlerMqtt(TransportFallbackType.WebSocketOnly);
                    break;
                case TransportType.Mqtt_Tcp_Only:
                    _transport = new ProvisioningTransportHandlerMqtt(TransportFallbackType.TcpOnly);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var provisioningClient = ProvisioningDeviceClient.Create(
                GlobalDeviceEndpoint,
                Device.AzureConnectionParameters.AzureScopeId, 
                _security,
                _transport);

            _logger?.LogDebug("Provisioning device.");
            var provisioningResult = await provisioningClient.RegisterAsync().ConfigureAwait(false);

            if (provisioningResult.Status == ProvisioningRegistrationStatusType.Assigned)
            {
                _logger.LogDebug("Successfully provisioned device. Creating client.");
                _deviceClient = DeviceClient.Create(
                    provisioningResult.AssignedHub,
                    new DeviceAuthenticationWithRegistrySymmetricKey(
                        Device.AzureConnectionParameters.AzureDeviceId,
                        _security.GetPrimaryKey()),
                    TransportType.Amqp);

                await _deviceClient.OpenAsync().ConfigureAwait(false);

                return true;
            }

            _logger.LogWarning(
                $"Failed to provision the device. {provisioningResult.Status} - {provisioningResult.ErrorMessage}. Disposing.");
            Dispose();
            return false;
        }

        public async Task PublishSelf()
        {
            await _deviceClient
                .SendEventAsync(new Message(Encoding.UTF8.GetBytes(Device.CreateMessagePayload())))
                .ConfigureAwait(false);
        }

        public async void Dispose()
        {
            _security?.Dispose();
            _transport?.Dispose();

            if (_deviceClient != null)
            {
                await _deviceClient.CloseAsync().ConfigureAwait(false);
                _deviceClient.Dispose();
            }
        }

        public override string ToString()
        {
            return $"{nameof(AzurePublisherDeviceHandle)} - {Device.AzureConnectionParameters.AzureDeviceId}";
        }
    }
}
