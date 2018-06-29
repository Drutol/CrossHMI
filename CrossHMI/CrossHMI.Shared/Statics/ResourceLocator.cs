using Autofac;
using CrossHMI.Shared.BL.Consumer;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.MessageHandling;
using UAOOI.Networking.UDPMessageHandler;

namespace CrossHMI.Shared.Statics
{
    public static class ResourceLocator
    {
        internal static ILifetimeScope _appLifetimeScope;

        internal static void RegisterResources(this ContainerBuilder builder)
        {
            //Library
            builder.RegisterType<MessageHandlerFactory>().As<IMessageHandlerFactory>().SingleInstance();
            builder.RegisterType<ConfigurationFactory>().As<IConfigurationFactory>().SingleInstance();
            builder.RegisterType<EncodingFactoryBinarySimple>().As<IEncodingFactory>().SingleInstance();
            builder.RegisterType<ConsumerBindingFactory>().As<IBindingFactory>().SingleInstance();

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