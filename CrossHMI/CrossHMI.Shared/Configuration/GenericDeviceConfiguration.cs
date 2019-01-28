using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Configuration
{
    public class GenericDeviceConfiguration
    {
        public Dictionary<string, BuiltInType> Properties { get; set; }
    }
}
