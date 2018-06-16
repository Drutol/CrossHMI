using Autofac;
using CrossHMI.Interfaces;
using CrossHMI.Shared.Composition;
using CrossHMI.Shared.Logging;

namespace CrossHMI.Shared.Statics
{
    public static class ResourceLocator
    {
        internal static ILifetimeScope _appLifetimeScope;

        internal static void RegisterResources(this ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(BuildCallback);

            builder.RegisterType<LibraryEventSourceConsumerAdapter>().As<ILibraryEventSourceConsumerAdapter>().SingleInstance();
            builder.RegisterType<LibraryCompositionProvider>().As<ILibraryCompositionProvider>().SingleInstance();
        }

        private static void BuildCallback(IContainer container)
        {
            _appLifetimeScope = container.BeginLifetimeScope();

            container.Resolve<ILibraryEventSourceConsumerAdapter>().StartListening();
        }

        public static ILifetimeScope ObtainScope()
        {
            return _appLifetimeScope.BeginLifetimeScope();
        }
    }
}