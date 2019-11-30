using System;
using System.Collections.Generic;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    ///     Interface used in the process of defining the <see cref="INetworkDevice" /> model.
    /// </summary>
    public interface INetworkDeviceDefinitionBuilder
    {
        /// <summary>
        ///     Define the subscription to given process variable found in repository.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="variableName">The name found in configuration.</param>
        /// <returns></returns>
        INetworkDeviceDefinitionBuilder DefineVariable<T>(string variableName);

        /// <summary>
        ///     Allows to define extension. Extension has to be defined in custom class inheriting from <see cref="ConfigurationData" />
        ///     as an <see cref="IEnumerable{T}" /> of <see cref="IAdditionalRepositoryDataDescriptor" />.
        ///     It will be matched against the configuration and callback will be fired.
        /// </summary>
        /// <typeparam name="TExtension">Extension type.</typeparam>
        /// <param name="extensionSelector">Delegate that chooses the list of elements contain the extension.</param>
        /// <param name="extenstionAssigned">Callback for when extension is assigned.</param>
        /// <returns></returns>
        INetworkDeviceDefinitionBuilder DefineConfigurationExtenstion<TExtension>(
            Func<ConfigurationData, IEnumerable<TExtension>> extensionSelector,
            Action<TExtension> extenstionAssigned)
            where TExtension : class, IAdditionalRepositoryDataDescriptor;
    }
}