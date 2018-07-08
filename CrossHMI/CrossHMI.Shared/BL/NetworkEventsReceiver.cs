using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Shared.BL.Consumer;
using CrossHMI.Shared.EventSources;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;
using UAOOI.Networking.SemanticData.MessageHandling;

namespace CrossHMI.Shared.BL
{
    public class NetworkEventsReceiver : DataManagementSetup, INetworkEventsReceiver
    {
        private readonly IRecordingBindingFactory _recordingBindingFactory;
        private readonly IDispatcherAdapter _dispatcherAdapter;

        public NetworkEventsReceiver(IRecordingBindingFactory bindingFactory, IConfigurationFactory configurationFactory,
            IMessageHandlerFactory messageHandlerFactory, IEncodingFactory encodingFactory, IDispatcherAdapter dispatcherAdapter)
        {
            _recordingBindingFactory = bindingFactory;
            _dispatcherAdapter = dispatcherAdapter;

            BindingFactory = bindingFactory;
            ConfigurationFactory = configurationFactory;
            MessageHandlerFactory = messageHandlerFactory;
            EncodingFactory = encodingFactory;
        }

        public async Task Initialize()
        {
            await Task.Run(() =>
            {
                Start();
            });         
        }

        public INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName)
        {
            var repositoryBindings = _recordingBindingFactory.GetConsumerBindingsForRepository(repository);
            var valueMonitor = repositoryBindings.First(pair => pair.Key.Equals(variableName)).Value;
            return new NetworkVariableEventSource<T>(valueMonitor, variableName);
        }

        public INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(string repository)
            where TDevice : INetworkDevice, new()
        {
            var source = new NetworkDeviceEventSource<TDevice>(_dispatcherAdapter);

            source.Device = new NetworkDeviceDefinitionBuilder<TDevice>(this, source, repository).Build();
                         
            return source;
        }

        class NetworkDeviceDefinitionBuilder<TDevice> : INetworkDeviceDefinitionBuilder where TDevice : INetworkDevice, new ()
        {
            private readonly INetworkEventsReceiver _parent;
            private readonly INetworkDeviceUpdateSourceBase _deviceUpdateSource;
            private readonly string _repository;

            public NetworkDeviceDefinitionBuilder(INetworkEventsReceiver parent,
                INetworkDeviceUpdateSourceBase deviceUpdateSource, string repository)
            {
                _parent = parent;
                _deviceUpdateSource = deviceUpdateSource;
                _repository = repository;
            }

            public INetworkDeviceDefinitionBuilder Define<T>(string variableName)
            {
                _deviceUpdateSource.RegisterNetworkVariable(_parent.ObtainEventSourceForVariable<T>(_repository,variableName));
                return this;
            }

            public TDevice Build()
            {
                var device = Activator.CreateInstance<TDevice>();
                device.AssignRepository(_repository);
                device.DefineVariables(this);
                return device;
            }
        }       
    }
}
