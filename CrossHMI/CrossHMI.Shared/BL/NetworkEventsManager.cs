using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.BL.Consumer;
using CrossHMI.Shared.Configuration;
using CrossHMI.Shared.EventSources;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;
using UAOOI.Networking.SemanticData.MessageHandling;

namespace CrossHMI.Shared.BL
{
    /// <inheritdoc cref="INetworkEventsManager"/>
    public partial class NetworkEventsManager<TConfiguration> : DataManagementSetup, INetworkEventsManager 
        where TConfiguration : ConfigurationData, new()
    {
        private readonly IRecordingBindingFactory _recordingBindingFactory;
        private readonly INetworkConfigurationProvider<TConfiguration> _configurationProvider;
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private readonly ILogAdapter<NetworkEventsManager<TConfiguration>> _logger;

        /// <summary>
        /// Creates new instance of <see cref="NetworkEventsManager{TConfiguration}"/>
        /// </summary>
        /// <param name="bindingFactory">Binding factory.</param>
        /// <param name="configurationFactory">Configuration factory.</param>
        /// <param name="configurationProvider">Configuration provider.</param>
        /// <param name="messageHandlerFactory">Message handler facory.</param>
        /// <param name="encodingFactory">Encoding factory.</param>
        /// <param name="dispatcherAdapter">Dispatcher adapter.</param>
        public NetworkEventsManager(
            IRecordingBindingFactory bindingFactory,
            IConfigurationFactory configurationFactory,
            INetworkConfigurationProvider<TConfiguration> configurationProvider,
            IMessageHandlerFactory messageHandlerFactory, 
            IEncodingFactory encodingFactory,
            IDispatcherAdapter dispatcherAdapter,
            ILogAdapter<NetworkEventsManager<TConfiguration>> logger)
        {
            _recordingBindingFactory = bindingFactory;
            _configurationProvider = configurationProvider;
            _dispatcherAdapter = dispatcherAdapter;
            _logger = logger;

            BindingFactory = bindingFactory;
            ConfigurationFactory = configurationFactory;
            MessageHandlerFactory = messageHandlerFactory;
            EncodingFactory = encodingFactory;
        }

        /// <inheritdoc />
        public async Task Initialize()
        {
            await Task.Run(() =>
            {
                Start();
            });         
        }

        /// <inheritdoc />
        public INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(string repository)
            where TDevice : INetworkDevice, new()
        {
            _logger.LogDebug($"Creating event source for: {repository}");
            var source = new NetworkDeviceEventSource<TDevice>(_dispatcherAdapter);

            source.Device = new NetworkDeviceDefinitionBuilder<TDevice>(this, source, repository).Build();

            _logger.LogDebug($"Created event source for: {repository}");
            return source;
        }

        private INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName)
        {
            var repositoryBindings = _recordingBindingFactory.GetConsumerBindingsForRepository(repository);
            var valueMonitor = repositoryBindings.First(pair => pair.Key.Equals(variableName)).Value;
            return new NetworkVariableEventSource<T>(valueMonitor, variableName);
        }
    }
}
