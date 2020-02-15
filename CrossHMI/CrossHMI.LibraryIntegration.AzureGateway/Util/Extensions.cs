using System;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;

namespace CrossHMI.LibraryIntegration.AzureGateway.Util
{
    internal static class Extensions
    {
        internal static void AssertNotNull(this IAzureDeviceParameters parameters)
        {
            if (parameters == null || 
                string.IsNullOrEmpty(parameters.AzureDeviceId) ||
                string.IsNullOrEmpty(parameters.AzureScopeId))
                throw new ArgumentException("Provided device does not had valid Azure connection parameters.");

        }
    }
}
