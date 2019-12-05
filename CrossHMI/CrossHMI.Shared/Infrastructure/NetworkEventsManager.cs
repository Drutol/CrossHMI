using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using Autofac;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Models.Networking;
using CrossHMI.Shared.EventSources;
using CrossHMI.Shared.Interfaces;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Core;
using UAOOI.Networking.SemanticData;

namespace CrossHMI.Shared.Infrastructure
{
    /// <inheritdoc cref="INetworkEventsManager" />
    public class NetworkEventsManager : DataManagementSetup, INetworkEventsManager
    {
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private readonly ILogAdapter<NetworkEventsManager> _logger;
        private readonly INetworkDeviceDefinitionBuilderFactory _deviceDefinitionBuilderFactory;
        private readonly ILifetimeScope _lifetimeScope;
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
        /// <param name="deviceDefinitionBuilderFactory">Definition builder factory.</param>
        public NetworkEventsManager(
            IRecordingBindingFactory bindingFactory,
            IConfigurationFactory configurationFactory,
            IMessageHandlerFactory messageHandlerFactory,
            IEncodingFactory encodingFactory,
            IDispatcherAdapter dispatcherAdapter,
            ILogAdapter<NetworkEventsManager> logger,
            INetworkDeviceDefinitionBuilderFactory deviceDefinitionBuilderFactory,
            ILifetimeScope lifetimeScope)
        {
            _recordingBindingFactory = bindingFactory;
            _dispatcherAdapter = dispatcherAdapter;
            _logger = logger;
            _deviceDefinitionBuilderFactory = deviceDefinitionBuilderFactory;
            _lifetimeScope = lifetimeScope;

            BindingFactory = bindingFactory;
            ConfigurationFactory = configurationFactory;
            MessageHandlerFactory = messageHandlerFactory;
            EncodingFactory = encodingFactory;

            bindingFactory.NewRepositoryReceived += BindingFactoryOnNewBindingCreatedForRepository;
            bindingFactory.NewBindingCreated += BindingFactoryOnNewBindingCreated;
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
            var source = new NetworkDeviceUpdateSource<TDevice>(_dispatcherAdapter);
            source.Device = ((NetworkDeviceDefinitionBuilder<TDevice>) _deviceDefinitionBuilderFactory
                .CreateBuilder<TDevice>(source)
                .WithRepository(repository)).Build(factory ?? (() => _lifetimeScope.Resolve<TDevice>()));
            
            _assignedRepositories[repository] = source.Device;
            _logger.LogDebug($"Created event source for: {repository}");
            return source;
        }

        /// <inheritdoc />
        public INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName)
        {
            var repositoryBindings = _recordingBindingFactory.GetConsumerBindingsForRepository(repository);
            var valueMonitor = repositoryBindings.First(pair => pair.Key.Equals(variableName)).Value;
            return new NetworkVariableEventSource<T>(valueMonitor, variableName);
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
    }
}