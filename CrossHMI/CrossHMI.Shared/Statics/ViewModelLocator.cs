using Autofac;
using CrossHMI.Shared.ViewModels;

namespace CrossHMI.Shared.Statics
{
    public static class ViewModelLocator
    {
        private static ILifetimeScope _appLifetimeScope;

        public static MainViewModel MainViewModel => _appLifetimeScope.Resolve<MainViewModel>();

        internal static void RegisterViewModels(this ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(BuildCallback);

            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<DashboardViewModel>().SingleInstance();
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