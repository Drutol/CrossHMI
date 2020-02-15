using System.Runtime.Serialization;
using System.Threading.Tasks;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace CrossHMI.AzureGatewayService.Infrastructure.Configuration
{
    /// <summary>
    ///     Class with extra data about the repository.
    /// </summary>
    [DataContract]
    public class BoilerRepositoryDetails :
        IAdditionalRepositoryDataDescriptor,
        IAzureDeviceParameters
    {
        public TransportType TransportType { get; } = TransportType.Amqp;

        [DataMember] public string AzureDeviceId { get; set; }
        [DataMember] public string AzureScopeId { get; set; }

        [DataMember] public string AzurePrimaryKey { get; set; }
        [DataMember] public string AzureSecondaryKey { get; set; }

        /// <inheritdoc />
        [DataMember]
        public string Repository { get; set; }

        public Task<SecurityProvider> GetSecurityProviderAsync() =>
            Task.FromResult<SecurityProvider>(new SecurityProviderSymmetricKey(
                AzureDeviceId,
                AzurePrimaryKey,
                AzureSecondaryKey));
    }
}