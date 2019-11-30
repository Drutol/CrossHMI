using System;
using System.Linq;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.EventSources;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.Core;
using UAOOI.Networking.SemanticData;

namespace CrossHMI.Shared.BL
{
    /// <inheritdoc cref="INetworkEventsManager" />
    public partial class NetworkEventsManager : DataManagementSetup, INetworkEventsManager
    {
        private readonly INetworkConfigurationProvider _configurationProvider;
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private readonly ILogAdapter<NetworkEventsManager> _logger;
        private readonly IRecordingBindingFactory _recordingBindingFactory;

        /// <summary>
        ///     Creates new instance of <see cref="NetworkEventsManager" />
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
            INetworkConfigurationProvider configurationProvider,
            IMessageHandlerFactory messageHandlerFactory,
            IEncodingFactory encodingFactory,
            IDispatcherAdapter dispatcherAdapter,
            ILogAdapter<NetworkEventsManager> logger)
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
            await Task.Run(() => { Start(); });
        }

        /// <inheritdoc />
        public INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(string repository,
            Func<TDevice> factory = null)
            where TDevice : INetworkDevice
        {
            _logger.LogDebug($"Creating event source for: {repository}");
            var source = new NetworkDeviceEventSource<TDevice>(_dispatcherAdapter);

            source.Device = new NetworkDeviceDefinitionBuilder<TDevice>(this, source, repository).Build(factory);

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