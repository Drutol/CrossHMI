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
    public class NetworkEventsManager : DataManagementSetup, INetworkEventsManager
    {
        private readonly ILogger<NetworkEventsManager> _logger;
        private readonly INetworkDeviceDefinitionBuilderFactory _deviceDefinitionBuilderFactory;
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
        /// <param name="messageHandlerFactory">Message handler factory.</param>
        /// <param name="encodingFactory">Encoding factory.</param>
        /// <param name="deviceDefinitionBuilderFactory">Definition builder factory.</param>
        public NetworkEventsManager(
            IRecordingBindingFactory bindingFactory,
            IConfigurationFactory configurationFactory,
            IMessageHandlerFactory messageHandlerFactory,
            IEncodingFactory encodingFactory,
            ILogger<NetworkEventsManager> logger,
            INetworkDeviceDefinitionBuilderFactory deviceDefinitionBuilderFactory)
        {
            _recordingBindingFactory = bindingFactory;
            _logger = logger;
            _deviceDefinitionBuilderFactory = deviceDefinitionBuilderFactory;

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
            await Task.Run(Start);
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
        public INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(
            string repository,
            Func<TDevice> factory)
            where TDevice : INetworkDevice
        {
            _logger.LogDebug($"Creating event source for: {repository}");
            var source = new NetworkDeviceUpdateSource<TDevice>();
            source.Device = ((NetworkDeviceDefinitionBuilder<TDevice>) _deviceDefinitionBuilderFactory
                .CreateBuilder<TDevice>(source)
                .WithRepository(repository)).Build(factory);
            
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