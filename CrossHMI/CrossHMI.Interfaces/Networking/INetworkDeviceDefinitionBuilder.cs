using System;
using System.Collections.Generic;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDeviceDefinitionBuilder<out TConfiguration> where TConfiguration : ConfigurationData
    {
        INetworkDeviceDefinitionBuilder<TConfiguration> DefineVariable<T>(string variableName);

        INetworkDeviceDefinitionBuilder<TConfiguration> DefineConfigurationExtenstion<TExtension>(
            Func<TConfiguration, IEnumerable<TExtension>> extensionSelector,
            Action<TExtension> extenstionAssigned)
            where TExtension : class, IAdditonalRepositoryDataDescriptor;

    }
}