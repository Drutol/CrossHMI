using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    ///     Interface for NetworkDevice that is instance of an object
    ///     directly related to and composed from given repository definition.
    /// </summary>
    public interface INetworkDevice
    {
        /// <summary>
        ///     Called during model creation. Provides currently associated repository.
        /// </summary>
        /// <param name="repository">The name of repository found in configuration.</param>
        void AssignRepository(string repository);

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
        /// <typeparam name="TConfiguration">Extended configuration type.</typeparam>
        /// <param name="builder">Builder to define the model.</param>
        void DefineDevice<TConfiguration>(INetworkDeviceDefinitionBuilder<TConfiguration> builder)
            where TConfiguration : ConfigurationData;
    }
}