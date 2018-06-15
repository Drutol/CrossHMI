using Autofac;

namespace CrossHMI.Shared.Statics
{
    public static class ResourceLocator
    {
        internal static ILifetimeScope _appLifetimeScope;

        internal static void RegisterResources(this ContainerBuilder builder)
        {
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