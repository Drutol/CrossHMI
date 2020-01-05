using System.Diagnostics;
using CrossHMI.LibraryIntegration.Interfaces;

namespace CrossHMI.Shared.Devices
{
    public class DynamicDevice : INetworkDynamicDevice
    {
        public string Repository { get; set; }

        public INetworkDeviceDynamicLifetimeHandle Handle { get; set; }

        public void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            Debug.WriteLine($"{Repository} - {variableName} = {value}");
        }

        public void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            Handle = builder.DeclareDynamic();
        }
    }
}
