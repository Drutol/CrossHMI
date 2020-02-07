using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.AzureGatewayService.Interfaces
{
    public interface IAzurePublisher
    {
        void RegisterDeviceForPublishing(IAzureEnabledNetworkDevice azureEnabledNetworkDevice);
    }
}
