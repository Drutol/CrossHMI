namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDeviceDefinitionBuilder
    {
        INetworkDeviceDefinitionBuilder Define<T>(string variableName);
    }
}
