namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDevice
    {
        void AssignRepository(string repository);
        void ProcessPropertyUpdate<T>(string variableName, T value);

        void DefineDevice(INetworkDeviceDefinitionBuilder builder);
    }
}
