using System;
using System.Collections.Generic;
using System.Text;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;

namespace CrossHMI.LibraryIntegration.AzureGateway.Util
{
    internal static class Extensions
    {
        internal static void AssertNotNull(this IAzureConnectionParameters parameters)
        {
            if (parameters == null || 
                string.IsNullOrEmpty(parameters.AzureDeviceId) ||
                string.IsNullOrEmpty(parameters.AzureScopeId) ||
                string.IsNullOrEmpty(parameters.AzurePrimaryKey) ||
                string.IsNullOrEmpty(parameters.AzureSecondaryKey))
                throw new ArgumentException("Provided device does not had valid Azure connection parameters.");

        }
    }
}
