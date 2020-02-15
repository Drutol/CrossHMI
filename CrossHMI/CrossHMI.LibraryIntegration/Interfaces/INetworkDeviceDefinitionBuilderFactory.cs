namespace CrossHMI.LibraryIntegration.Interfaces
{
    /// <summary>
    /// This component is factory for <see cref="INetworkDeviceDefinitionBuilder"/> of given device type.
    /// </summary>
    public interface INetworkDeviceDefinitionBuilderFactory
    {
        INetworkDeviceDefinitionBuilder CreateBuilder<T>(INetworkDeviceUpdateSourceBase source) where T : INetworkDevice;
    }
}