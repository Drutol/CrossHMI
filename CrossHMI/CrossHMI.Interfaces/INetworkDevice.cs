using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkDevice
    {
        void AssignRepository(string repository);
        void ProcessPropertyUpdate<T>(string variableName, T value);

        void DefineVariables(INetworkDeviceDefinitionBuilder builder);
    }
}
