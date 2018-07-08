using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkDeviceDefinitionBuilder
    {
        INetworkDeviceDefinitionBuilder Define<T>(string variableName);
    }
}
