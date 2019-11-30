using System;
using System.Runtime.Serialization;
using System.Xml;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using UAOOI.Configuration.Networking;

namespace CrossHMI.Shared.BL.Consumer
{
    /// <summary>
    ///     Class responsible for loading and propagating configuration of the library.
    /// </summary>
    public class ConfigurationFactory : ConfigurationFactoryBase<BoilersConfigurationData>,
        INetworkConfigurationProvider<BoilersConfigurationData>
    {
        private readonly IConfigurationResourcesProvider _configurationResourcesProvider;
        private readonly ILogAdapter<ConfigurationFactory> _logger;

        /// <summary>
        ///     Creates new instaince of <see cref="ConfigurationFactory" />
        /// </summary>
        /// <param name="configurationResourcesProvider">The provider of raw configuration asset.</param>
        public ConfigurationFactory(IConfigurationResourcesProvider configurationResourcesProvider,
            ILogAdapter<ConfigurationFactory> logger)
        {
            _configurationResourcesProvider = configurationResourcesProvider;
            _logger = logger;
            Loader = ConfigurationLoader;
        }

        /// <inheritdoc />
        public BoilersConfigurationData CurrentConfiguration { get; private set; }

        private BoilersConfigurationData ConfigurationLoader()
        {
            _logger.LogDebug("Configuration data has been requested.");
            return ConfigurationDataFactoryIO.Load(DataLoader, RaiseEvents);
        }

        private BoilersConfigurationData DataLoader()
        {
            _logger.LogDebug("Loading data from registered adapter.");
            using (var reader =
                new XmlTextReader(_configurationResourcesProvider.ObtainLibraryConfigurationXML()))
            {
                _logger.LogDebug("Deserializing XML configuration.");
                var configuration = new DataContractSerializer(typeof(BoilersConfigurationData))
                    .ReadObject(reader, false) as BoilersConfigurationData;

                CurrentConfiguration = configuration;
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