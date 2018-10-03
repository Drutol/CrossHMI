using Autofac;
using CrossHMI.Shared.ViewModels;

namespace CrossHMI.Shared.Statics
{
    /// <summary>
    /// Class responsible for providing management and static access to ViewModels.
    /// </summary>
    public static class ViewModelLocator
    {
        private static ILifetimeScope _appLifetimeScope;

        /// <summary>
        /// Gets the singleton instance of <see cref="MainViewModel"/>
        /// </summary>
        public static MainViewModel MainViewModel => _appLifetimeScope.Resolve<MainViewModel>();

        /// <summary>
        /// Registers ViewModels found in application.
        /// </summary>
        /// <param name="builder"></param>
        internal static void RegisterViewModels(this ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(BuildCallback);

            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<DashboardViewModel>().SingleInstance();
            builder.RegisterType<BoilerDetailsViewModel>().SingleInstance();
        }

        private static void BuildCallback(IContainer container)
        {
            _appLifetimeScope = container.BeginLifetimeScope();
        }
    }
}