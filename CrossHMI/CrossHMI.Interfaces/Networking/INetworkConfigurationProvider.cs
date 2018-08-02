using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkConfigurationProvider<out T> where T : ConfigurationData
    {
        T CurrentConfiguration { get; }
    }
}
