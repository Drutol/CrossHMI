using System.Collections.Generic;
using CrossHMI.AzureGatewayService.Interfaces;
using CrossHMI.LibraryIntegration.Interfaces;

namespace CrossHMI.AzureGatewayService.Infrastructure.Configuration
{
    /// <summary>
    ///     Class with extra data about the repository.
    /// </summary>
    public class BoilerRepositoryDetails : IAdditionalRepositoryDataDescriptor, IAzureConnectionParameters
    {
        public string AzureDeviceId { get; set; }

        public string AzureScopeId { get; set; }

        public string AzurePrimaryKey { get; set; }

        public string AzureSecondaryKey { get; set; }

        /// <inheritdoc />
        public string Repository { get; set; }
    }
}