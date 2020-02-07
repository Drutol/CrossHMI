using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.AzureGatewayService.Interfaces
{
    public interface IAzureConnectionParameters
    {
        string AzureDeviceId { get; }

        string AzureScopeId { get; }

        string AzurePrimaryKey { get; }

        string AzureSecondaryKey { get; }
    }
}
