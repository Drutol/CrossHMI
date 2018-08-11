using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    /// Interface for distributing loaded configuration between oher components.
    /// </summary>
    /// <typeparam name="T">Type of the extended configuration.</typeparam>
    public interface INetworkConfigurationProvider<out T> where T : ConfigurationData
    {
        /// <summary>
        /// Get current configuration.
        /// </summary>
        T CurrentConfiguration { get; }
    }
}
