using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Statics;
using UAOOI.Configuration.Networking.Serialization;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CrossHMI.Shared.BL
{
    public partial class NetworkEventsManager<TConfiguration>
    {
        /// <summary>
        /// Interface for objects representing registration of an extension.
        /// </summary>
        interface IExtensionDeclaration
        {
            /// <summary>
            /// Used to find proper extension object and pass
            /// it to undrelying device via provided callback delegate.
            /// </summary>
            void Assign();
        }

        /// <summary>
        /// Builder that allows the model class in question
        /// define itself without exposing unnecessary information.
        /// </summary>
        /// <typeparam name="TDevice"></typeparam>
        internal class NetworkDeviceDefinitionBuilder<TDevice> : INetworkDeviceDefinitionBuilder<TConfiguration>
            where TDevice : INetworkDevice, new()
        {
            private static Func<TDevice> DefaultDeviceFactory { get; set; } = Activator.CreateInstance<TDevice>;
            private static ILogAdapter<NetworkDeviceDefinitionBuilder<TDevice>> DefaultLogger { get; set; }

            private readonly NetworkEventsManager<TConfiguration> _parent;
            private readonly INetworkDeviceUpdateSourceBase _deviceUpdateSource;
            private readonly string _repository;

            private Func<TDevice> _deviceInstanceFactory;
            private readonly List<IExtensionDeclaration> _extensionDeclarations = new List<IExtensionDeclaration>();
            private readonly ILogAdapter<NetworkDeviceDefinitionBuilder<TDevice>> _builderLogger;

            /// <summary>
            /// Creates new instance of <see cref="NetworkDeviceDefinitionBuilder{TDevice}"/>
            /// </summary>
            /// <param name="parent">Parent <see cref="NetworkEventsManager{TConfiguration}"/></param>
            /// <param name="deviceUpdateSource">The device update source.</param>
            /// <param name="repository">The repository associated with the device.</param>
            /// <param name="deviceInstanceFactory">Delegate that will be used to instantiate bare device class instance. By default <see cref="Activator.CreateInstance{T}"/> is used.</param>
            public NetworkDeviceDefinitionBuilder(NetworkEventsManager<TConfiguration> parent,
                INetworkDeviceUpdateSourceBase deviceUpdateSource,
                string repository,
                Func<TDevice> deviceInstanceFactory = null)
            {
                try
                {
                    _builderLogger = ResourceLocator.GetLogger<NetworkDeviceDefinitionBuilder<TDevice>>();
                }
                catch
                { 
                    _builderLogger = DefaultLogger;
                }
                
                _parent = parent;
                _deviceUpdateSource = deviceUpdateSource;
                _repository = repository;
                _deviceInstanceFactory = deviceInstanceFactory ?? DefaultDeviceFactory;
            }

            /// <inheritdoc />
            public INetworkDeviceDefinitionBuilder<TConfiguration> DefineVariable<T>(string variableName)
            {
                _builderLogger.LogDebug($"Defining {variableName} of type {typeof(T).Name} for {_repository}.");
                _deviceUpdateSource.RegisterNetworkVariable(_parent.ObtainEventSourceForVariable<T>(_repository, variableName));
                return this;
            }

            /// <inheritdoc />
            public INetworkDeviceDefinitionBuilder<TConfiguration> DefineConfigurationExtenstion<TExtension>(
                Func<TConfiguration, IEnumerable<TExtension>> extensionSelector,
                Action<TExtension> extenstionAssigned)
                where TExtension : class, IAdditonalRepositoryDataDescriptor
            {
                _builderLogger.LogDebug($"Defining configuration extension of type {typeof(TExtension).Name} for {_repository}.");
                _extensionDeclarations.Add(
                    new ExtensionDeclaration<TExtension>(this, extensionSelector, extenstionAssigned));
                return this;
            }

            /// <summary>
            /// Instantiates and defines the device.
            /// </summary>
            /// <returns></returns>
            public TDevice Build()
            {
                _builderLogger.LogDebug($"Commencing building event source for {_repository}");
                var device = _deviceInstanceFactory();
                _builderLogger.LogDebug($"Instantiated device model.");
                device.AssignRepository(_repository);
                _builderLogger.LogDebug($"Assigned repository.");
                if (typeof(INetworkDeviceWithConfiguration<TConfiguration>).IsAssignableFrom(typeof(TDevice)))
                {
                    ((INetworkDeviceWithConfiguration<TConfiguration>)device).DefineDevice(this);
                }
                else
                {
                    device.DefineDevice(this);
                }
                _builderLogger.LogDebug($"Finished defining device.");
                foreach (var extensionDeclaration in _extensionDeclarations)
                {
                    _builderLogger.LogDebug($"Assigning extension matched with {_repository} repository.");
                    extensionDeclaration.Assign();
                }

                _builderLogger.LogDebug($"Finished building device for {_repository}");
                return device;
            }

#region TestInstrumentation

[Conditional("DEBUG")]
internal static void OverrideDefaultDeviceFactory(Func<TDevice> factory)
{
    DefaultDeviceFactory = factory;
}

[Conditional("DEBUG")]
internal static void OverrideDefaultLogger(ILogAdapter<NetworkDeviceDefinitionBuilder<TDevice>> logger)
{
    DefaultLogger = logger;
}

#endregion

            /// <summary>
            /// Helper class for storing data about extensions passed to builer while defining the model.
            /// </summary>
            /// <typeparam name="TExtension"></typeparam>
            class ExtensionDeclaration<TExtension> : IExtensionDeclaration
                where TExtension : class, IAdditonalRepositoryDataDescriptor
            {
                private readonly NetworkDeviceDefinitionBuilder<TDevice> _parent;
                private readonly Func<TConfiguration, IEnumerable<object>> _extensionSelector;
                private readonly Action<TExtension> _extenstionAssigned;

                /// <summary>
                /// Creates new <see cref="ExtensionDeclaration{TExtension}"/> instance.
                /// </summary>
                /// <param name="parent">Parent <see cref="NetworkDeviceDefinitionBuilder{TDevice}"/></param>
                /// <param name="extensionSelector">Selector of extensions collection.</param>
                /// <param name="extenstionAssigned">Callback for when the extension is found.</param>
                public ExtensionDeclaration(NetworkDeviceDefinitionBuilder<TDevice> parent,
                    Func<TConfiguration, IEnumerable<object>> extensionSelector,
                    Action<TExtension> extenstionAssigned)
                {
                    _parent = parent;
                    _extensionSelector = extensionSelector;
                    _extenstionAssigned = extenstionAssigned;
                }

                /// <inheritdoc />
                public void Assign()
                {
                    var extensions =
                        _extensionSelector(_parent._parent._configurationProvider.CurrentConfiguration as TConfiguration)
                            .Cast<IAdditonalRepositoryDataDescriptor>();
                    var assignedExtension =
                        extensions.FirstOrDefault(descriptor => descriptor.Repository.Equals(_parent._repository));

                    _extenstionAssigned(assignedExtension as TExtension);
                }
            }
        }
    }
}
