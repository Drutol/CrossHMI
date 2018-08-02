using System;
using System.Collections.Generic;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDeviceDefinitionBuilder
    {
        INetworkDeviceDefinitionBuilder DefineVariable<T>(string variableName);

        INetworkDeviceDefinitionBuilder DefineConfigurationExtenstion<TConfiguration, TExtension>(
            Func<TConfiguration, IEnumerable<TExtension>> extensionSelector, 
            Action<TExtension> extenstionAssigned)
            where TExtension : class, IAdditonalRepositoryDataDescriptor 
            where TConfiguration : ConfigurationData;
    }
}