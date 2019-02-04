using System.Linq;
using System.Reflection;
using Autofac;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.BL;
using CrossHMI.Shared.BL.Consumer;
using CrossHMI.Shared.Configuration;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Core;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.MessageHandling;
using UAOOI.Networking.UDPMessageHandler;

namespace CrossHMI.Shared.Statics
{
    /// <summary>
    /// Class responsible for providing management and static access
    /// to app resources whenever it's not feasible for them to be injected via Dependency inejction.
    /// </summary>
    public static class ResourceLocator
    {
        private static ILifetimeScope _appLifetimeScope;

        /// <summary>
        /// Registers all non ViewModel components that will be used in the application.
        /// </summary>
        /// <param name="builder"></param>
        internal static void RegisterResources(this ContainerBuilder builder)
        {
            //Library
            builder.RegisterType<MessageHandlerFactory>()
                .As<IMessageHandlerFactory>()
                .SingleInstance();

            builder.RegisterType<ConfigurationFactory>()
                .As<IConfigurationFactory>()
                .As<INetworkConfigurationProvider<BoilersConfigurationData>>()
                .SingleInstance();

            builder.RegisterType<EncodingFactoryBinarySimple>()
                .As<IEncodingFactory>()
                .SingleInstance();

            builder.RegisterType<ConsumerBindingFactory>()
                .As<IRecordingBindingFactory>()
                .SingleInstance();

            //Library Orchiestrastion
            builder.RegisterType<NetworkEventsManager<BoilersConfigurationData>>()
                .As<INetworkEventsManager>()
                .SingleInstance();

            builder.RegisterBuildCallback(BuildCallback);
        }

        public static ILogAdapter<T> GetLogger<T>() => _appLifetimeScope.Resolve<ILogAdapter<T>>();

        /// <summary>
        /// Allows to obtain resouce scope for manual component resolution.
        /// </summary>
        /// <returns>New resource scope.</returns>
        public static ILifetimeScope ObtainScope()
        {
            return _appLifetimeScope.BeginLifetimeScope();
        }

        private static void BuildCallback(IContainer container)
        {
            _appLifetimeScope = container.BeginLifetimeScope();


            var logger = GetLogger<IContainer>();
            var assemblies = new[]
            {
                Assembly.GetAssembly(typeof(MessageHandlerFactory)),
                Assembly.GetAssembly(typeof(IConfigurationFactory)),
                Assembly.GetAssembly(typeof(IBindingFactory)),
                Assembly.GetAssembly(typeof(EncodingFactoryBinarySimple)),
            }.Select(assembly => assembly.FullName);
            logger.LogDebug($"UAOOI Assemblies containing registered components in the IoC container:\n{string.Join("\n", assemblies)}");
        }
    }
}