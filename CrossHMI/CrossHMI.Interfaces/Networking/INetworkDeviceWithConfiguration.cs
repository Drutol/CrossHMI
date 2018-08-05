using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDeviceWithConfiguration<in TConfiguration> : INetworkDevice
        where TConfiguration : ConfigurationData
    {
        void DefineDevice(INetworkDeviceDefinitionBuilder<TConfiguration> builder);
    }
}
