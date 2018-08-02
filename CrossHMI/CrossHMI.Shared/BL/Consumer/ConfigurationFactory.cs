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
    public class ConfigurationFactory : ConfigurationFactoryBase<BoilersConfigurationData> , INetworkConfigurationProvider<BoilersConfigurationData>
    {
        private readonly IConfigurationResourcesProvider _configurationResourcesProvider;

        public BoilersConfigurationData CurrentConfiguration { get; private set; }

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

        protected override void RaiseEvents()
        {
            OnAssociationConfigurationChange?.Invoke(this,EventArgs.Empty);
            OnMessageHandlerConfigurationChange?.Invoke(this,EventArgs.Empty);
        }

        public override event EventHandler<EventArgs> OnAssociationConfigurationChange;
        public override event EventHandler<EventArgs> OnMessageHandlerConfigurationChange;
    
    }
}
