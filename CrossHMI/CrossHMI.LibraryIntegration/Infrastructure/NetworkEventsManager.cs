using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossHMI.LibraryIntegration.Infrastructure.EventSources;
using CrossHMI.LibraryIntegration.Interfaces;
using CrossHMI.LibraryIntegration.Models;
using Microsoft.Extensions.Logging;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Core;
using UAOOI.Networking.SemanticData;

namespace CrossHMI.LibraryIntegration.Infrastructure
{
    /// <inheritdoc cref="INetworkEventsManager" />
    public class NetworkEventsManager : INetworkEventsManager
    {
        public event EventHandler<INetworkDeviceUpdateSource<INetworkDynamicDevice>> NewDeviceCreated;

        private readonly ILogger<NetworkEventsManager> _logger;
        private readonly INetworkDeviceDefinitionBuilderFactory _deviceDefinitionBuilderFactory;
        private readonly AutoWiredDataManagementSetup _dataManagementSetup;

        private readonly IRecordingBindingFactory _recordingBindingFactory;

        private readonly Dictionary<string, INetworkDevice> _assignedRepositories 
            = new Dictionary<string, INetworkDevice>();

        private bool _isDynamicInstantiationEnabled;
        private Func<INetworkDynamicDevice> _dynamicDeviceFactory;

        /// <summary>
        /// Creates new instance of <see cref="NetworkEventsManager" />
        /// </summary>
        /// <param name="bindingFactory">Binding factory.</param>
        /// <param name="configurationFactory">Configuration factory.</param>
        /// <param name="messageHandlerFactory">Message handler factory.</param>
        /// <param name="encodingFactory">Encoding factory.</param>
        /// <param name="deviceDefinitionBuilderFactory">Definition builder factory.</param>
        /// <param name="dataManagementSetup">UAOOI's DataManagementSetup setup.</param>
        /// <param name="logger">Optional logger instance.</param>
        public NetworkEventsManager(
            IRecordingBindingFactory bindingFactory,
            INetworkDeviceDefinitionBuilderFactory deviceDefinitionBuilderFactory,
            AutoWiredDataManagementSetup dataManagementSetup,
            ILogger<NetworkEventsManager> logger = null)
        {
            _recordingBindingFactory = bindingFactory;
            _logger = logger;
            _deviceDefinitionBuilderFactory = deviceDefinitionBuilderFactory;
            _dataManagementSetup = dataManagementSetup;

            bindingFactory.NewRepositoryReceived += BindingFactoryOnNewBindingCreatedForRepository;
            bindingFactory.NewBindingCreated += BindingFactoryOnNewBindingCreated;
        }

        /// <inheritdoc />
        public async Task Initialize()
        {
            await Task.Run(_dataManagementSetup.Run);
        }

        /// <inheritdoc />
        public void EnableAutomaticDeviceInstantiation(Func<INetworkDynamicDevice> deviceFactory)
        {
            _isDynamicInstantiationEnabled = true;
            _dynamicDeviceFactory = deviceFactory;
        }

        /// <inheritdoc />
        public void DisableAutomaticDeviceInstantiation()
        {
            _isDynamicInstantiationEnabled = false;
            _dynamicDeviceFactory = null;
        }

        /// <inheritdoc />
        public INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(
            string repository,
            Func<TDevice> factory)
            where TDevice : INetworkDevice
        {
            _logger?.LogDebug($"Creating event source for: {repository}");
            var source = new NetworkDeviceUpdateSource<TDevice>();
            source.Device = ((NetworkDeviceDefinitionBuilder<TDevice>) _deviceDefinitionBuilderFactory
                .CreateBuilder<TDevice>(source)
                .WithRepository(repository)).Build(factory);
            
            _assignedRepositories[repository] = source.Device;
            _logger?.LogDebug($"Created event source for: {repository}");
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
                NewDeviceCreated?.Invoke(this, ObtainEventSourceForDevice(repository, () => _dynamicDeviceFactory()));
            }
        }
    }
}