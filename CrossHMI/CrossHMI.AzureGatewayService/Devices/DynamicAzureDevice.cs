using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CrossHMI.AzureGatewayService.Infrastructure.Configuration;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using CrossHMI.LibraryIntegration.Infrastructure.Devices;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.IoT.DigitalTwin.Parser;
using Microsoft.Azure.IoT.DigitalTwin.Parser.Contracts;
using Microsoft.Azure.IoT.DigitalTwin.Parser.Vocabulary;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Optional;
using Optional.Collections;

namespace CrossHMI.AzureGatewayService.Devices
{
    public class DynamicAzureDevice : NetworkDeviceBase, INetworkDynamicDevice, IAzureEnabledNetworkDevice
    {
        private readonly ILogger<DynamicAzureDevice> _logger;

        public DeviceClient DeviceClient { get; set; }
        public INetworkDeviceDynamicLifetimeHandle Handle { get; private set; }
        public TimeSpan PublishingInterval { get; private set; }
        public IAzureDeviceParameters AzureDeviceParameters { get; private set; }
        
        private readonly Dictionary<string, string> _deviceState = new Dictionary<string, string>();
        private Option<CapabilityModel> _capabilityModel;
        private string _repository;

        public DynamicAzureDevice(ILogger<DynamicAzureDevice> logger)
        {
            _logger = logger;
        }

        public override string Repository
        {
            get => _repository;
            set
            {
                _repository = value;
                _logger.BeginScope($"Device:{value}");
            }
        }

        public override void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            _capabilityModel.Match(model =>
            {
                model.Implements
                    .SelectMany(instance => instance.Interface.Telemetries)
                    .FirstOrNone(telemetry => telemetry.Name == variableName)
                    .MatchSome(telemetry =>
                    {
                        SchemaTypeToObjectType(telemetry.Schema.SchemaType).MatchSome(type =>
                        {
                            if (type == typeof(T))
                                _deviceState[variableName] = value.ToString();
                        });
                    });
            }, () => { _deviceState[variableName] = value.ToString(); });
        }

        public string CreateMessagePayload()
        {
            _logger.LogTrace("Building payload.");
            return JsonConvert.SerializeObject(_deviceState);
        }

        public override void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            builder
                .DeclareDynamic(handle => Handle = handle)
                .RequestConfigurationExtenstion<BoilerRepositoryDetails>(data =>
                {
                    AzureDeviceParameters = data;
                    PublishingInterval = data.PublishingInterval;
                    if (data.CapabilityModelPath != null)
                    {
                        _capabilityModel = Option.None<CapabilityModel>();
                        _logger.LogInformation("Parsing capability model.");
                        try
                        {
                            var modelParserOutput = new ModelParser().Parse(File.ReadAllText(data.CapabilityModelPath));
                            if (modelParserOutput.IsSuccessful &&
                                modelParserOutput.Model is CapabilityModel capabilityModel)
                            {
                                _logger.LogInformation("Successfully parsed capability model.");
                                _capabilityModel = capabilityModel.Some();
                                if (modelParserOutput.ParserValidation?.Errors.Any() ?? false)
                                {
                                    _logger.LogWarning(
                                        $"Parsed model contains {modelParserOutput.ParserValidation.Errors.Count} validation errors.");
                                }
                            }
                            else
                            {
                                _logger.LogWarning("Failed to parse capability model.");
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Failed to load capability model.");
                        }
                    }
                    else
                    {
                        _capabilityModel = Option.None<CapabilityModel>();
                    }
                });
        }

        private Option<Type> SchemaTypeToObjectType(SchemaType schemaType) => schemaType switch
        {
            SchemaType.Unknown => Option.None<Type>(),
            SchemaType.Boolean => typeof(bool).Some(),
            SchemaType.Date => typeof(DateTime).Some(),
            SchemaType.DateTime => typeof(DateTime).Some(),
            SchemaType.Double => typeof(double).Some(),
            SchemaType.Duration => typeof(TimeSpan).Some(),
            SchemaType.Float => typeof(float).Some(),
            SchemaType.Integer => typeof(int).Some(),
            SchemaType.Long => typeof(long).Some(),
            SchemaType.String => typeof(string).Some(),
            SchemaType.Time => typeof(TimeSpan).Some(),
            SchemaType.Enum => Option.None<Type>(),
            SchemaType.Array => Option.None<Type>(),
            SchemaType.Object => Option.None<Type>(),
            SchemaType.Map => Option.None<Type>(),
            SchemaType.Geospatial => Option.None<Type>(),
            SchemaType.InterfaceInstance => Option.None<Type>(),
            _ => throw new ArgumentOutOfRangeException(nameof(schemaType), schemaType, null)
        };
    }
}
