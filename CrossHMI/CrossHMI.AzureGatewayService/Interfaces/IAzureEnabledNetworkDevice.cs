using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.AzureGatewayService.Interfaces
{
    public interface IAzureEnabledNetworkDevice
    {
        IAzureConnectionParameters AzureConnectionParameters { get; }

        string CreateMessagePayload();
    }
}
