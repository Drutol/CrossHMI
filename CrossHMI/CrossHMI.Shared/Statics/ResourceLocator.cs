using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using CrossHMI.LibraryIntegration.Infrastructure;
using CrossHMI.LibraryIntegration.Interfaces;
using CrossHMI.Shared.Infrastructure;
using CrossHMI.Shared.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Core;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.UDPMessageHandler;

namespace CrossHMI.Shared.Statics
{
    /// <summary>
    ///     Class responsible for providing management and static access
    ///     to app resources whenever it's not feasible for them to be injected via Dependency inejction.
    /// </summary>
    public static class ResourceLocator
    {
        private static ILifetimeScope _appLifetimeScope;

        /// <summary>
        ///     Registers all non ViewModel components that will be used in the application.
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
                .As<IAdditionalRepositoryDescriptorProvider>()
                .SingleInstance();

            builder.RegisterType<EncodingFactoryBinarySimple>()
                .As<IEncodingFactory>()
                .SingleInstance();

            builder.RegisterType<ConsumerBindingFactory>()
                .As<IRecordingBindingFactory>()
                .SingleInstance();

            //Library Orchestration
            builder.RegisterType<NetworkEventsManager>()
                .As<INetworkEventsManager>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(NetworkDeviceDefinitionBuilder<>));
            builder.RegisterType<NetworkDeviceDefinitionBuilderFactory>()
                .As<INetworkDeviceDefinitionBuilderFactory>();
            builder.RegisterBuildCallback(scope => _appLifetimeScope = scope);

            builder.Register(context => new LoggerFactory(context.Resolve<IEnumerable<ILoggerProvider>>())).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
        }

        /// <summary>
        ///     Allows to obtain resource scope for manual component resolution.
        /// </summary>
        /// <returns>New resource scope.</returns>
        public static ILifetimeScope ObtainScope()
        {
            return _appLifetimeScope.BeginLifetimeScope();
        }

        private static void BuildCallback(IContainer container)
        {
            _appLifetimeScope = container.BeginLifetimeScope();


            var logger = _appLifetimeScope.Resolve<ILogger<IContainer>>();
            var assemblies = new[]
            {
                Assembly.GetAssembly(typeof(MessageHandlerFactory)),
                Assembly.GetAssembly(typeof(IConfigurationFactory)),
                Assembly.GetAssembly(typeof(IBindingFactory)),
                Assembly.GetAssembly(typeof(EncodingFactoryBinarySimple))
            }.Select(assembly => assembly.FullName);
            logger.LogDebug(
                $"UAOOI Assemblies containing registered components in the IoC container:\n{string.Join("\n", assemblies)}");
        }
    }
}