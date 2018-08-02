using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.BL
{
    public partial class NetworkEventsReceiver
    {
        class NetworkDeviceDefinitionBuilder<TDevice> : INetworkDeviceDefinitionBuilder 
            where TDevice : INetworkDevice, new()
        {
            private readonly NetworkEventsReceiver _parent;
            private readonly INetworkDeviceUpdateSourceBase _deviceUpdateSource;
            private readonly string _repository;

            private readonly List<IExtensionDeclaration> _extensionDeclarations = new List<IExtensionDeclaration>();

            public NetworkDeviceDefinitionBuilder(NetworkEventsReceiver parent,
                INetworkDeviceUpdateSourceBase deviceUpdateSource, string repository)
            {
                _parent = parent;
                _deviceUpdateSource = deviceUpdateSource;
                _repository = repository;
            }

            public INetworkDeviceDefinitionBuilder DefineVariable<T>(string variableName)
            {
                _deviceUpdateSource.RegisterNetworkVariable(_parent.ObtainEventSourceForVariable<T>(_repository, variableName));
                return this;
            }

            public INetworkDeviceDefinitionBuilder DefineConfigurationExtenstion<TConfiguration, TExtension>(
                Func<TConfiguration, IEnumerable<TExtension>> extensionSelector,
                Action<TExtension> extenstionAssigned)
                where TConfiguration : ConfigurationData
                where TExtension : class, IAdditonalRepositoryDataDescriptor
            {
                _extensionDeclarations.Add(new ExtensionDeclaration<TConfiguration, TExtension>(this)
                {
                    Selector = extensionSelector,
                    AssignDelegate = extenstionAssigned,
                });
                return this;
            }

            public TDevice Build()
            {
                var device = Activator.CreateInstance<TDevice>();
                device.AssignRepository(_repository);
                device.DefineDevice(this);

                foreach (var extensionDeclaration in _extensionDeclarations)
                    extensionDeclaration.Assign();

                return device;
            }



            interface IExtensionDeclaration
            {
                void Assign();
            }

            class ExtensionDeclaration<TConfiguration, TExtension> : IExtensionDeclaration
                where TExtension : class, IAdditonalRepositoryDataDescriptor
                where TConfiguration : ConfigurationData
            {
                private readonly NetworkDeviceDefinitionBuilder<TDevice> _parent;

                public ExtensionDeclaration(NetworkDeviceDefinitionBuilder<TDevice> parent)
                {
                    _parent = parent;
                }

                public Func<TConfiguration, IEnumerable<TExtension>> Selector { get; set; }
                public Action<TExtension> AssignDelegate { get; set; }

                public void Assign()
                {
                    var extensions =
                        Selector(_parent._parent._configurationProvider.CurrentConfiguration as TConfiguration)
                            .Cast<IAdditonalRepositoryDataDescriptor>();
                    var assignedExtension =
                        extensions.FirstOrDefault(descriptor => descriptor.Repository.Equals(_parent._repository));

                    AssignDelegate(assignedExtension as TExtension);
                }
            }
        }
    }
}
