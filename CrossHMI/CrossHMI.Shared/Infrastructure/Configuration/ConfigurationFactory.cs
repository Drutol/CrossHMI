using System;
using System.Collections.Generic;
using System.IO;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using Newtonsoft.Json;
using UAOOI.Configuration.Networking;

namespace CrossHMI.Shared.Infrastructure.Configuration
{
    /// <summary>
    ///     Class responsible for loading and propagating configuration of the library.
    /// </summary>
    public class ConfigurationFactory :
        ConfigurationFactoryBase<BoilersConfigurationData>,
        IAdditionalRepositoryDescriptorProvider
    {
        private readonly IConfigurationResourcesProvider _configurationResourcesProvider;
        private readonly ILogAdapter<ConfigurationFactory> _logger;

        /// <inheritdoc />
        public IReadOnlyCollection<IAdditionalRepositoryDataDescriptor> Descriptors { get; private set; }

        /// <summary>
        ///     Creates new instance of <see cref="ConfigurationFactory" />
        /// </summary>
        /// <param name="configurationResourcesProvider">The provider of raw configuration asset.</param>
        public ConfigurationFactory(
            IConfigurationResourcesProvider configurationResourcesProvider,
            ILogAdapter<ConfigurationFactory> logger)
        {
            _configurationResourcesProvider = configurationResourcesProvider;
            _logger = logger;
            Loader = ConfigurationLoader;
        }

        private BoilersConfigurationData ConfigurationLoader()
        {
            _logger.LogDebug("Configuration data has been requested.");
            return ConfigurationDataFactoryIO.Load(DataLoader, RaiseEvents);
        }

        private BoilersConfigurationData DataLoader()
        {
            _logger.LogDebug("Loading data from registered adapter.");
            using (var reader =
                new StreamReader(_configurationResourcesProvider.ObtainLibraryConfiguration()))
            {
                _logger.LogDebug("Deserializing JSON configuration.");
                var configuration = JsonConvert.DeserializeObject<BoilersConfigurationData>(reader.ReadToEnd(), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                Descriptors = configuration.AdditionalRepositoryData;
                _logger.LogDebug("Configuration successfully deserialized.");
                return configuration;
            }
        }

        /// <inheritdoc />
        protected override void RaiseEvents()
        {
            OnAssociationConfigurationChange?.Invoke(this, EventArgs.Empty);
            OnMessageHandlerConfigurationChange?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public override event EventHandler<EventArgs> OnAssociationConfigurationChange;

        /// <inheritdoc />
        public override event EventHandler<EventArgs> OnMessageHandlerConfigurationChange;
    }
}