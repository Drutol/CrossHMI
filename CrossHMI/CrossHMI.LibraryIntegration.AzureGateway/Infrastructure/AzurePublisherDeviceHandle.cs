﻿using System;
using System.Text;
using System.Threading;
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
    public class AzurePublisherDeviceHandle : IAsyncDisposable
    {
        private const string GlobalDeviceEndpoint = "global.azure-devices-provisioning.net";

        private SecurityProvider _security;
        private ProvisioningTransportHandler _transport;
        private DeviceClient _deviceClient;
        private readonly ILogger<AzurePublisherDeviceHandle> _logger;
        private Timer _timer;

        public IAzureEnabledNetworkDevice Device { get; }

        public AzurePublisherDeviceHandle(
            IAzureEnabledNetworkDevice device,
            ILogger<AzurePublisherDeviceHandle> logger)
        {
            _logger = logger;
            device.AzureDeviceParameters.AssertNotNull();

            Device = device;
            _logger.BeginScope($"Device:{Device.AzureDeviceParameters.AzureDeviceId}");
        }

        public async Task<bool> Initialize()
        {
            _logger.LogDebug("Obtaining security provider for the device.");
            _security = await Device.AzureDeviceParameters.GetSecurityProviderAsync().ConfigureAwait(false);

            switch (Device.AzureDeviceParameters.TransportType)
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
                Device.AzureDeviceParameters.AzureScopeId,
                _security,
                _transport);

            _logger?.LogDebug("Provisioning device.");
            var provisioningResult = await provisioningClient.RegisterAsync().ConfigureAwait(false);

            if (provisioningResult.Status == ProvisioningRegistrationStatusType.Assigned)
            {
                _logger.LogDebug("Successfully provisioned device. Creating client.");
                IAuthenticationMethod auth;
                switch (_security)
                {
                    case SecurityProviderTpm tpmSecurity:
                        auth = new DeviceAuthenticationWithTpm(
                            Device.AzureDeviceParameters.AzureDeviceId,
                            tpmSecurity);
                        break;
                    case SecurityProviderX509 certificateSecurity:
                        auth = new DeviceAuthenticationWithX509Certificate(
                            Device.AzureDeviceParameters.AzureDeviceId,
                            certificateSecurity.GetAuthenticationCertificate());
                        break;
                    case SecurityProviderSymmetricKey symmetricKeySecurity:
                        auth = new DeviceAuthenticationWithRegistrySymmetricKey(
                            Device.AzureDeviceParameters.AzureDeviceId,
                            symmetricKeySecurity.GetPrimaryKey());
                        break;
                    default:
                        _logger.LogError("Specified security provider is unknown.");
                        throw new NotSupportedException("Unknown authentication type.");
                }

                _deviceClient = DeviceClient.Create(
                    provisioningResult.AssignedHub,
                    auth,
                    Device.AzureDeviceParameters.TransportType);

                await _deviceClient.OpenAsync().ConfigureAwait(false);

                Device.DeviceClient = _deviceClient;

                return true;
            }

            _logger.LogWarning(
                $"Failed to provision the device. {provisioningResult.Status} - {provisioningResult.ErrorMessage}. Disposing.");
            await DisposeAsync();
            return false;
        }

        public void StartPublishing()
        {
            _timer = new Timer(Callback, null, TimeSpan.Zero, Device.PublishingInterval);
        }

        private async void Callback(object state)
        {
            try
            {
                await PublishSelf();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to publish device state.");
            }
        }

        public async Task PublishSelf()
        {
            await _deviceClient
                .SendEventAsync(new Message(Encoding.UTF8.GetBytes(Device.CreateMessagePayload())))
                .ConfigureAwait(false);
            _logger.LogDebug("Successfully published device state to Azure.");
        }

        public override string ToString()
        {
            return $"{nameof(AzurePublisherDeviceHandle)} - {Device.AzureDeviceParameters.AzureDeviceId}";
        }

        public async ValueTask DisposeAsync()
        {
            _security?.Dispose();
            _transport?.Dispose();
            _timer?.DisposeAsync();

            if (_deviceClient != null)
            {
                Device.DeviceClient = null;
                try
                {
                    await _deviceClient.CloseAsync().ConfigureAwait(false);
                    _logger.LogInformation($"Disposed azure connection.");
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Azure connection already disposed.");
                }

                _deviceClient.Dispose();
            }
        }
    }
}
