using System;
using CrossHMI.Interfaces.Networking;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Devices
{
    /// <summary>
    ///     Utility class built on top of <see cref="NetworkDeviceBase" />.
    ///     It's meant to hide <see cref="INetworkDevice.DefineDevice{TConfiguration}" />
    ///     and implement <see cref="INetworkDeviceWithConfiguration{TConfiguration}.DefineDevice" />
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    public abstract class NetworkDeviceBaseWithConfiguration<TConfiguration>
        : NetworkDeviceBase, INetworkDeviceWithConfiguration<TConfiguration>
        where TConfiguration : ConfigurationData
    {
        /// <inheritdoc />
        public virtual void DefineDevice(INetworkDeviceDefinitionBuilder<TConfiguration> builder)
        {
            base.DefineDevice(builder);
        }

        /// <inheritdoc />
        public sealed override void DefineDevice<TConfig>(INetworkDeviceDefinitionBuilder<TConfig> builder)
        {
            throw new NotImplementedException(
                "This method should not be called since the method with generic configuration is already defined.");
        }
    }
}