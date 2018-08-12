using System;
using Autofac;

namespace CrossHMI.Shared.Statics
{
    /// <summary>
    /// Static class serving as a shared entry point for the application.
    /// </summary>
    public static class AppInitializationRoutines
    {
        /// <summary>
        /// Initializes the application and configures resource used for dependency injection further on.
        /// </summary>
        /// <param name="adaptersRegistration">Delegate allowing to register implementations of shared interfaces.</param>
        public static void Init(Action<ContainerBuilder> adaptersRegistration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterResources();
            builder.RegisterViewModels();
            adaptersRegistration(builder);

            builder.Build();
        }
    }
}