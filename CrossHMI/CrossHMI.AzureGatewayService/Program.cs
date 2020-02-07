using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossHMI.AzureGatewayService.Infrastructure;
using CrossHMI.AzureGatewayService.Infrastructure.Configuration;
using CrossHMI.LibraryIntegration.Infrastructure;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.Core;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.UDPMessageHandler;

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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Bootstrapper>();
                    services.AddHostedService<AzurePublisher>();

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

                    services.AddTransient(typeof(NetworkDeviceDefinitionBuilder<>));
                })
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                });
    }
}
