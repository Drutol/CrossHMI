using System;
using System.Collections.Generic;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    /// Interface used in the process of defining the <see cref="INetworkDevice"/> model.
    /// </summary>
    /// <typeparam name="TConfiguration">Current configuration type.</typeparam>
    public interface INetworkDeviceDefinitionBuilder<out TConfiguration> where TConfiguration : ConfigurationData
    {
        /// <summary>
        /// Define the subscription to given process variable found in repository.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="variableName">The name found in configuration.</param>
        /// <returns></returns>
        INetworkDeviceDefinitionBuilder<TConfiguration> DefineVariable<T>(string variableName);

        /// <summary>
        /// Allows to define extension. Extension has to be defined in inherited <see cref="TConfiguration"/>
        /// as an <see cref="IEnumerable{T}"/> of <see cref="IAdditonalRepositoryDataDescriptor"/>.
        /// It will be matched agains the configuration and callback will be fired.
        /// </summary>
        /// <typeparam name="TExtension">Extension type.</typeparam>
        /// <param name="extensionSelector">Delegate that chooses the list of elements containg the extension.</param>
        /// <param name="extenstionAssigned">Callback for when extension is assigned.</param>
        /// <returns></returns>
        INetworkDeviceDefinitionBuilder<TConfiguration> DefineConfigurationExtenstion<TExtension>(
            Func<TConfiguration, IEnumerable<TExtension>> extensionSelector,
            Action<TExtension> extenstionAssigned)
            where TExtension : class, IAdditonalRepositoryDataDescriptor;

    }
}