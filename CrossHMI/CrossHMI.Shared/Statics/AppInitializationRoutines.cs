using System;
using Autofac;

namespace CrossHMI.Shared.Statics
{
    public static class AppInitializationRoutines
    {
        public static void Init(Action<ContainerBuilder> adaptersRegistration)
        {
            //configure dependencies
            var builder = new ContainerBuilder();

            builder.RegisterResources();
            builder.RegisterViewModels();
            adaptersRegistration(builder);

            builder.Build();
        }
    }
}