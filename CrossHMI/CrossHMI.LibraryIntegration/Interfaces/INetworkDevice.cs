﻿namespace CrossHMI.LibraryIntegration.Interfaces
{
    /// <summary>
    ///     Interface for NetworkDevice that is instance of an object
    ///     directly related to and composed from given repository definition.
    /// </summary>
    public interface INetworkDevice
    {
        /// <summary>
        ///     Provided during model creation.
        /// </summary>
        string Repository { get; set; }

        /// <summary>
        ///     Called whenever new property value for given repository is availble.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="variableName">Variable name found in configuration.</param>
        /// <param name="value">Actual new value.</param>
        void ProcessPropertyUpdate<T>(string variableName, T value);

        /// <summary>
        ///     Called in order to utilise <see cref="builder" />
        ///     to define the model with properties and configuration extensions.
        /// </summary>
        /// <param name="builder">Builder to define the model.</param>
        void DefineDevice(INetworkDeviceDefinitionBuilder builder);
    }
}