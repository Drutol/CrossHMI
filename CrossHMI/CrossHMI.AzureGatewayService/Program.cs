using System;
using System.Diagnostics.Tracing;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CrossHMI.AzureGatewayService.Devices;
using CrossHMI.AzureGatewayService.Infrastructure;
using CrossHMI.AzureGatewayService.Infrastructure.Configuration;
using CrossHMI.AzureGatewayService.Interfaces;
using CrossHMI.LibraryIntegration.AzureGateway.Infrastructure;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using CrossHMI.LibraryIntegration.Infrastructure;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using Serilog.Events;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Core;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.UDPMessageHandler;
using UAOOI.Networking.UDPMessageHandler.Diagnostic;

namespace CrossHMI.AzureGatewayService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(context => new AutofacServiceProviderFactory(ConfigurationContainer))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IAzurePublisher, AzurePublisher>();
                    services.AddSingleton<IBootstrapper, Bootstrapper>();

                    services.AddHostedService(provider => (Bootstrapper) provider.GetRequiredService<IBootstrapper>());

                    //Library
                    services.AddSingleton<IMessageHandlerFactory, MessageHandlerFactory>();
                    services.AddSingleton<IConfigurationFactory, ConfigurationFactory>();
                    services.AddSingleton<IAdditionalRepositoryDescriptorProvider>(provider =>
                        (ConfigurationFactory) provider.GetRequiredService<IConfigurationFactory>());
                    services.AddSingleton<IEncodingFactory, EncodingFactoryBinarySimple>();
                    services.AddSingleton<IRecordingBindingFactory, ConsumerBindingFactory>();
                    services.AddSingleton<INetworkEventsManager, NetworkEventsManager>();
                    services
                        .AddSingleton<INetworkDeviceDefinitionBuilderFactory, NetworkDeviceDefinitionBuilderFactory>();

                    services.AddSingleton<ILibraryLogger, LibraryLogger>();

                    services.AddTransient(typeof(NetworkDeviceDefinitionBuilder<>));
                    services.AddTransient<Boiler>();

                    services.Remove(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
                    services.AddTransient(typeof(ILogger<>), typeof(ScopedLogger<>));
                })
                .UseSerilog((context, configuration) =>
                {
                    var logTemplate =
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message} {Exception} {Properties} {NewLine}";
                    configuration
                        .Enrich.FromLogContext()
                        .MinimumLevel.Verbose()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .WriteTo.Console(LogEventLevel.Verbose, logTemplate);
                });

        private static void ConfigurationContainer(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .As(typeof(INetworkingEventSourceProvider));
        }
    }
}
