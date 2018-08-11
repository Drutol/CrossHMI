using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Configuration.Networking.Serializers;

namespace CrossHMI.Shared.BL.Consumer
{
    /// <summary>
    /// Class responsible for loading and propagating configuration of the library.
    /// </summary>
    public class ConfigurationFactory : ConfigurationFactoryBase<BoilersConfigurationData> , INetworkConfigurationProvider<BoilersConfigurationData>
    {
        private readonly IConfigurationResourcesProvider _configurationResourcesProvider;

        /// <inheritdoc />
        public BoilersConfigurationData CurrentConfiguration { get; private set; }

        /// <summary>
        /// Creates new instaince of <see cref="ConfigurationFactory"/>
        /// </summary>
        /// <param name="configurationResourcesProvider">The provider of raw configuration asset.</param>
        public ConfigurationFactory(IConfigurationResourcesProvider configurationResourcesProvider)
        {
            _configurationResourcesProvider = configurationResourcesProvider;
            Loader = ConfigurationLoader;
        }

        private BoilersConfigurationData ConfigurationLoader()
        {
            return ConfigurationDataFactoryIO.Load(DataLoader, RaiseEvents);
        }

        private BoilersConfigurationData DataLoader()
        {
            using (var reader =
                new XmlTextReader(_configurationResourcesProvider.ObtainLibraryConfigurationXML()))
            {
                var configuration = new DataContractSerializer(typeof(BoilersConfigurationData))
                    .ReadObject(reader, false) as BoilersConfigurationData;

                CurrentConfiguration = configuration;

                return configuration;
            }
        }

        /// <inheritdoc />
        protected override void RaiseEvents()
        {
            OnAssociationConfigurationChange?.Invoke(this,EventArgs.Empty);
            OnMessageHandlerConfigurationChange?.Invoke(this,EventArgs.Empty);
        }

        /// <inheritdoc />
        public override event EventHandler<EventArgs> OnAssociationConfigurationChange;

        /// <inheritdoc />
        public override event EventHandler<EventArgs> OnMessageHandlerConfigurationChange;
    
    }
}
