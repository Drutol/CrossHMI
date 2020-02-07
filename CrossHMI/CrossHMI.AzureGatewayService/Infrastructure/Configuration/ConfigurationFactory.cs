using System;
using System.Collections.Generic;
using System.IO;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UAOOI.Configuration.Networking;

namespace CrossHMI.AzureGatewayService.Infrastructure.Configuration
{
    /// <summary>
    ///     Class responsible for loading and propagating configuration of the library.
    /// </summary>
    public class ConfigurationFactory :
        ConfigurationFactoryBase<BoilersConfigurationData>,
        IAdditionalRepositoryDescriptorProvider
    {
        private readonly ILogger<ConfigurationFactory> _logger;

        /// <inheritdoc />
        public IReadOnlyCollection<IAdditionalRepositoryDataDescriptor> Descriptors { get; private set; }

        /// <summary>
        ///     Creates new instance of <see cref="ConfigurationFactory" />
        /// </summary>
        public ConfigurationFactory(
            ILogger<ConfigurationFactory> logger)
        {
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
                new StreamReader(File.OpenRead("Configuration/LibraryConfiguration.json")))
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