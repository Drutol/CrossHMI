using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Extensions.Logging;

namespace CrossHMI.LibraryIntegration.Infrastructure
{
    /// <summary>
    ///     Builder that allows the model class in question
    ///     define itself without exposing unnecessary information.
    /// </summary>
    /// <typeparam name="TDevice"></typeparam>
    public class NetworkDeviceDefinitionBuilder<TDevice> : INetworkDeviceDefinitionBuilder
        where TDevice : INetworkDevice
    {
        private readonly ILogger<NetworkDeviceDefinitionBuilder<TDevice>> _builderLogger;
        private readonly List<IExtensionDeclaration> _extensionDeclarations = new List<IExtensionDeclaration>();
        private readonly INetworkEventsManager _networkEventsManager;
        private readonly IAdditionalRepositoryDescriptorProvider _additionalRepositoryDescriptorProvider;

        private string _repository;
        private INetworkDeviceUpdateSourceBase _deviceUpdateSource;

        private readonly Func<TDevice> _deviceInstanceFactory = DefaultDeviceFactory;
        private NetworkDeviceDynamicLifetimeHandle _dynamicHandle;

        /// <summary>
        ///     Creates new instance of <see cref="NetworkDeviceDefinitionBuilder{TDevice}" />
        /// </summary>
        /// <param name="networkEventsManager">Network events manager.</param>
        /// <param name="additionalRepositoryDescriptorProvider">Additional descriptor provider.</param>
        /// <param name="deviceUpdateSource">The device update source.</param>
        /// <param name="logger">Logger.</param>
        public NetworkDeviceDefinitionBuilder(
            INetworkEventsManager networkEventsManager,
            IAdditionalRepositoryDescriptorProvider additionalRepositoryDescriptorProvider,
            INetworkDeviceUpdateSourceBase deviceUpdateSource,
            ILogger<NetworkDeviceDefinitionBuilder<TDevice>> logger = null)
        {
            _builderLogger = logger;
            _networkEventsManager = networkEventsManager;
            _additionalRepositoryDescriptorProvider = additionalRepositoryDescriptorProvider;

        }

        private static Func<TDevice> DefaultDeviceFactory { get; set; } = Activator.CreateInstance<TDevice>;

        private static ILogger<NetworkDeviceDefinitionBuilder<TDevice>> DefaultLogger { get; set; }

        /// <inheritdoc />
        public INetworkDeviceDefinitionBuilder WithRepository(string repository)
        {
            _repository = repository;
            return this;
        }

        /// <inheritdoc />
        public INetworkDeviceDefinitionBuilder WithUpdateSource(INetworkDeviceUpdateSourceBase deviceUpdateSource)
        {
            _deviceUpdateSource = deviceUpdateSource;
            return this;
        }

        /// <inheritdoc />
        public INetworkDeviceDefinitionBuilder DefineVariable<T>(string variableName)
        {
            if (_deviceUpdateSource == null)
                throw new InvalidOperationException("Unable to define variable without defining update source.");

            _builderLogger.LogDebug($"Defining {variableName} of type {typeof(T).Name} for {_repository}.");
            _deviceUpdateSource.RegisterNetworkVariable(
                _networkEventsManager.ObtainEventSourceForVariable<T>(_repository, variableName));
            return this;
        }

        /// <inheritdoc />
        public INetworkDeviceDefinitionBuilder RequestConfigurationExtenstion<TExtension>(
            Action<TExtension> extenstionAssigned)
            where TExtension : class, IAdditionalRepositoryDataDescriptor
        {
            _builderLogger.LogDebug(
                $"Defining configuration extension of type {typeof(TExtension).Name} for {_repository}.");
            _extensionDeclarations.Add(
                new ExtensionDeclaration<TExtension>(this, _additionalRepositoryDescriptorProvider, extenstionAssigned));
            return this;
        }

        /// <inheritdoc />
        public INetworkDeviceDynamicLifetimeHandle DeclareDynamic()
        {
            _dynamicHandle =  new NetworkDeviceDynamicLifetimeHandle(_networkEventsManager);
            return _dynamicHandle;
        }

        /// <summary>
        ///     Instantiates and defines the device.
        /// </summary>
        /// <param name="factory"></param>
        public TDevice Build(Func<TDevice> factory = null)
        {
            if (_deviceUpdateSource == null)
                throw new InvalidOperationException("Unable to build the device without defining update source.");

            _builderLogger.LogDebug($"Commencing building event source for {_repository}");
            var device = (factory ?? _deviceInstanceFactory)();
            _builderLogger.LogDebug("Instantiated device model.");
            device.Repository = _repository;
            _builderLogger.LogDebug("Assigned repository.");

            device.DefineDevice(this);

            _builderLogger.LogDebug("Finished defining device.");
            foreach (var extensionDeclaration in _extensionDeclarations)
            {
                _builderLogger.LogDebug($"Assigning extension matched with {_repository} repository.");
                extensionDeclaration.Assign();
            }

            if (_dynamicHandle != null)
            {
                _dynamicHandle.DeviceUpdateSourceBase = _deviceUpdateSource;
            }

            _builderLogger.LogDebug($"Finished building device for {_repository}");
            return device;
        }

        /// <summary>
        ///     Helper class for storing data about extensions passed to builder while defining the model.
        /// </summary>
        /// <typeparam name="TExtension"></typeparam>
        private class ExtensionDeclaration<TExtension> : IExtensionDeclaration
            where TExtension : class, IAdditionalRepositoryDataDescriptor
        {
            
            private readonly Action<TExtension> _extenstionAssigned;
            private readonly NetworkDeviceDefinitionBuilder<TDevice> _parent;
            private readonly IAdditionalRepositoryDescriptorProvider _additionalRepositoryDescriptorProvider;

            /// <summary>
            ///     Creates new <see cref="ExtensionDeclaration{TExtension}" /> instance.
            /// </summary>
            /// <param name="parent">Parent <see cref="NetworkDeviceDefinitionBuilder{TDevice}" /></param>
            /// <param name="additionalRepositoryDescriptorProvider">Provider of extensions collection.</param>
            /// <param name="extenstionAssigned">Callback for when the extension is found.</param>
            public ExtensionDeclaration(
                NetworkDeviceDefinitionBuilder<TDevice> parent,
                IAdditionalRepositoryDescriptorProvider additionalRepositoryDescriptorProvider,
                Action<TExtension> extenstionAssigned)
            {
                _parent = parent;
                _additionalRepositoryDescriptorProvider = additionalRepositoryDescriptorProvider;

                _extenstionAssigned = extenstionAssigned;
            }

            /// <inheritdoc />
            public void Assign()
            {
                var assignedExtension =
                    _additionalRepositoryDescriptorProvider
                        .Descriptors?
                        .FirstOrDefault(descriptor => descriptor.Repository.Equals(_parent._repository));

                _extenstionAssigned(assignedExtension as TExtension);
            }
        }

        #region TestInstrumentation

        [Conditional("DEBUG")]
        internal static void OverrideDefaultDeviceFactory(Func<TDevice> factory)
        {
            DefaultDeviceFactory = factory;
        }

        [Conditional("DEBUG")]
        internal static void OverrideDefaultLogger(ILogger<NetworkDeviceDefinitionBuilder<TDevice>> logger)
        {
            DefaultLogger = logger;
        }

        #endregion
    }

}