using System;
using System.Collections.Generic;
using System.Text;
using CrossHMI.Interfaces.Networking;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Devices
{
    public abstract class NetworkDeviceBaseWithConfiguration<TConfiguration>
        : NetworkDeviceBase, INetworkDeviceWithConfiguration<TConfiguration>
        where TConfiguration : ConfigurationData
    {
        public virtual void DefineDevice(INetworkDeviceDefinitionBuilder<TConfiguration> builder)
        {
            base.DefineDevice(builder);
        }

        public sealed override void DefineDevice<TConfig>(INetworkDeviceDefinitionBuilder<TConfig> builder)
        {

        }
    }
}
