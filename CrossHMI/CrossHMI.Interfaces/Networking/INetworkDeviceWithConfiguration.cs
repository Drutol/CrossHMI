using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    /// Generic version of <see cref="INetworkDevice"/> allowing to pass builder with generic type in order to avoid casting.
    /// </summary>
    /// <typeparam name="TConfiguration">Current configuration.</typeparam>
    public interface INetworkDeviceWithConfiguration<in TConfiguration> : INetworkDevice
        where TConfiguration : ConfigurationData
    {
        /// <summary>
        /// Called in order to utilise <see cref="builder"/>
        /// to define the model with properties and configuration extensions.
        /// </summary>
        /// <typeparam name="TConfiguration">Extended configuration type.</typeparam>
        /// <param name="builder">Builder to define the model.</param>
        void DefineDevice(INetworkDeviceDefinitionBuilder<TConfiguration> builder);
    }
}
