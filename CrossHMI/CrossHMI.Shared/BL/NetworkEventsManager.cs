using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Models.Networking;
using CrossHMI.Shared.EventSources;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.Core;
using UAOOI.Networking.SemanticData;

namespace CrossHMI.Shared.BL
{
    /// <inheritdoc cref="INetworkEventsManager" />
    public class NetworkEventsManager : DataManagementSetup, INetworkEventsManager
    {
        internal readonly INetworkConfigurationProvider _configurationProvider;
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private readonly ILogAdapter<NetworkEventsManager> _logger;
        private readonly IRecordingBindingFactory _recordingBindingFactory;

        private readonly Dictionary<string, INetworkDevice> _assignedRepositories 
            = new Dictionary<string, INetworkDevice>();

        private bool _isDynamicInstantiationEnabled;
        private Type _dynamicInstantiationDeviceType;

        /// <summary>
        ///     Creates new instance of <see cref="NetworkEventsManager" />
        /// </summary>
        /// <param name="bindingFactory">Binding factory.</param>
        /// <param name="configurationFactory">Configuration factory.</param>
        /// <param name="configurationProvider">Configuration provider.</param>
        /// <param name="messageHandlerFactory">Message handler factory.</param>
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

            bindingFactory.NewRepositoryReceived += BindingFactoryOnNewBindingCreatedForRepository;
            bindingFactory.NewBindingCreated += BindingFactoryOnNewBindingCreated;
        }

        private void BindingFactoryOnNewBindingCreated(object sender, CreateBindingEventArgs e)
        {
            if (_isDynamicInstantiationEnabled)
            {
                if (_assignedRepositories.TryGetValue(e.Repository, out var device))
                {
                    if (device is INetworkDynamicDevice dynamicDevice)
                    {
                        dynamicDevice.Handle.NotifyNewBindingCreated(e.Repository, e.ProcessValue, e.BindingType);
                    }
                }
            }
        }

        private void BindingFactoryOnNewBindingCreatedForRepository(object sender, string repository)
        {
            if (_isDynamicInstantiationEnabled && !_assignedRepositories.ContainsKey(repository))
            {
                ObtainEventSourceForDevice(repository, () => (INetworkDynamicDevice)Activator.CreateInstance(_dynamicInstantiationDeviceType));
            }
        }

        /// <inheritdoc />
        public async Task Initialize()
        {
            await Task.Run(() => { Start(); });
        }

        public void EnableAutomaticDeviceInstantiation<TDevice>() where TDevice : INetworkDynamicDevice, new()
        {
            _isDynamicInstantiationEnabled = true;
            _dynamicInstantiationDeviceType = typeof(TDevice);
        }

        public void DisableAutomaticDeviceInstantiation()
        {
            _isDynamicInstantiationEnabled = false;
            _dynamicInstantiationDeviceType = null;
        }

        /// <inheritdoc />
        public INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(string repository,
            Func<TDevice> factory = null)
            where TDevice : INetworkDevice
        {
            _logger.LogDebug($"Creating event source for: {repository}");
            var source = new NetworkDeviceEventSource<TDevice>(_dispatcherAdapter);

            source.Device = new NetworkDeviceDefinitionBuilder<TDevice>(this, source, repository).Build(factory);
            _assignedRepositories[repository] = source.Device;

            _logger.LogDebug($"Created event source for: {repository}");
            return source;
        }

        internal INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName)
        {
            var repositoryBindings = _recordingBindingFactory.GetConsumerBindingsForRepository(repository);
            var valueMonitor = repositoryBindings.First(pair => pair.Key.Equals(variableName)).Value;
            return new NetworkVariableEventSource<T>(valueMonitor, variableName);
        }
    }
}