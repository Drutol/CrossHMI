using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using CrossHMI.Interfaces.Adapters;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Configuration.Networking.Serializers;

namespace CrossHMI.Shared.BL.Consumer
{
    public class ConfigurationFactory : ConfigurationFactoryBase
    {
        private readonly IConfigurationResourcesProvider _configurationResourcesProvider;

        public ConfigurationFactory(IConfigurationResourcesProvider configurationResourcesProvider)
        {
            _configurationResourcesProvider = configurationResourcesProvider;
            Loader = ConfigurationLoader;
        }

        private ConfigurationData ConfigurationLoader()
        {
            return ConfigurationDataFactoryIO.Load(DataLoader, RaiseEvents);
        }

        private ConfigurationData DataLoader()
        {
            using (var reader =
                new XmlTextReader(_configurationResourcesProvider.ObtainLibraryConfigurationXML()))
            {
                return new DataContractSerializer(typeof(ConfigurationData))
                        .ReadObject(reader, false) as ConfigurationData;
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
