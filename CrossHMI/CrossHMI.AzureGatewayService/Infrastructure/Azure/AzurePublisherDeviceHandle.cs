using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CrossHMI.AzureGatewayService.Devices;
using CrossHMI.AzureGatewayService.Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace CrossHMI.AzureGatewayService.Infrastructure.Azure
{
    internal class AzurePublisherDeviceHandle : IDisposable
    {
        private const string GlobalDeviceEndpoint = "global.azure-devices-provisioning.net";

        private readonly IAzureEnabledNetworkDevice _device;
        private SecurityProviderSymmetricKey _security;
        private ProvisioningTransportHandlerAmqp _transport;
        private DeviceClient _deviceClient;

        public AzurePublisherDeviceHandle(IAzureEnabledNetworkDevice device)
        {
            _device = device;
        }

        public async Task<bool> Initialize()
        {
            _security = new SecurityProviderSymmetricKey(
                _device.AzureConnectionParameters.AzureDeviceId,
                _device.AzureConnectionParameters.AzurePrimaryKey,
                _device.AzureConnectionParameters.AzureSecondaryKey);

            _transport = new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly);

            var provisioningClient = ProvisioningDeviceClient.Create(
                GlobalDeviceEndpoint,
                _device.AzureConnectionParameters.AzureScopeId, 
                _security,
                _transport);

            var provisioningResult = await provisioningClient.RegisterAsync().ConfigureAwait(false);

            if (provisioningResult.Status == ProvisioningRegistrationStatusType.Assigned)
            {
                _deviceClient = DeviceClient.Create(
                    provisioningResult.AssignedHub,
                    new DeviceAuthenticationWithRegistrySymmetricKey(
                        _device.AzureConnectionParameters.AzureDeviceId,
                        _security.GetPrimaryKey()),
                    TransportType.Http1);

                await _deviceClient.OpenAsync().ConfigureAwait(false);

                return true;
            }

            Dispose();
            return false;
        }

        public async Task PublishSelf()
        {
            await _deviceClient
                .SendEventAsync(new Message(Encoding.UTF8.GetBytes(_device.CreateMessagePayload())))
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
            return $"{nameof(AzurePublisherDeviceHandle)} - {_device.AzureConnectionParameters.AzureDeviceId}";
        }
    }
}
