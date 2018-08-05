using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDevice
    {
        void AssignRepository(string repository);
        void ProcessPropertyUpdate<T>(string variableName, T value);

        void DefineDevice<TConfiguration>(INetworkDeviceDefinitionBuilder<TConfiguration> builder) 
            where TConfiguration : ConfigurationData;
    }
}
