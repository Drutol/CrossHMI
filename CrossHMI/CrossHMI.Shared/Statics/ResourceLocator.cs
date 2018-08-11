using Autofac;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.BL;
using CrossHMI.Shared.BL.Consumer;
using CrossHMI.Shared.Configuration;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.MessageHandling;
using UAOOI.Networking.UDPMessageHandler;

namespace CrossHMI.Shared.Statics
{
    public static class ResourceLocator
    {
        private static ILifetimeScope _appLifetimeScope;

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
            builder.RegisterType<NetworkEventsReceiver<BoilersConfigurationData>>()
                .As<INetworkEventsReceiver>()
                .SingleInstance();

            builder.RegisterBuildCallback(BuildCallback);
        }

        private static void BuildCallback(IContainer container)
        {
            _appLifetimeScope = container.BeginLifetimeScope();
        }

        public static ILifetimeScope ObtainScope()
        {
            return _appLifetimeScope.BeginLifetimeScope();
        }
    }
}