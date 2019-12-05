using System.Collections.Generic;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Infrastructure.Configuration
{
    public class GenericDeviceConfiguration
    {
        public Dictionary<string, BuiltInType> Properties { get; set; }
    }
}