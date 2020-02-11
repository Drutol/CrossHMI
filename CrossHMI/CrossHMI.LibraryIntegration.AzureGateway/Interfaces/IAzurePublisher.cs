using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace CrossHMI.LibraryIntegration.AzureGateway.Interfaces
{
    public interface IAzurePublisher
    {
        TimeSpan PublishInterval { get; set; }

        Task CancelDevicePublishingAsync(IAzureEnabledNetworkDevice device);
        Task<bool> RegisterDeviceForPublishingAsync(IAzureEnabledNetworkDevice device);

        Task StartAsync(CancellationToken cancellationToken);
    }
}
