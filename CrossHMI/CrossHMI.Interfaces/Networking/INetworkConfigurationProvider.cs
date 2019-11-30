using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    ///     Interface for distributing loaded configuration between oher components.
    /// </summary>
    public interface INetworkConfigurationProvider
    {
        /// <summary>
        ///     Get current configuration.
        /// </summary>
        ConfigurationData CurrentConfiguration { get; }
    }
}