using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CrossHMI.AzureGatewayService.Infrastructure.Configuration;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using CrossHMI.LibraryIntegration.Infrastructure.Devices;
using CrossHMI.LibraryIntegration.Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CrossHMI.AzureGatewayService.Devices
{
    /// <summary>
    ///     Model class representing the boiler repository. This class can be treated as ViewModel with implemented
    ///     <see cref="INotifyPropertyChanged" />.
    /// </summary>
    [DataContract]
    public class Boiler : NetworkDeviceBase, IAzureEnabledNetworkDevice
    {
        private readonly ILogger<Boiler> _logger;
        private string _repository;
        public DeviceClient DeviceClient { get; set; }

        public TimeSpan PublishingInterval => ConfigurationData.PublishingInterval;

        public override string Repository
        {
            get => _repository;
            set
            {
                _repository = value;
                _logger.BeginScope(value);
            }
        }

        public BoilerRepositoryDetails ConfigurationData { get; private set; }
        public IAzureDeviceParameters AzureDeviceParameters => ConfigurationData;

        public Boiler(ILogger<Boiler> logger)
        {
            _logger = logger;
        }

        //InputPipe
        [DataMember] [ProcessVariable] public double PipeX001_FTX001_Output { get; private set; }

        [DataMember] [ProcessVariable] public double PipeX001_ValveX001_Input { get; private set; }

        //Drum
        [DataMember] [ProcessVariable] public double DrumX001_LIX001_Output { get; private set; }

        //OutputPipe
        [DataMember] [ProcessVariable] public double PipeX002_FTX002_Output { get; private set; }

        //FlowController
        [DataMember] [ProcessVariable] public double FCX001_ControlOut { get; private set; }
        [DataMember] [ProcessVariable] public double FCX001_Measurement { get; private set; }

        [DataMember] [ProcessVariable] public double FCX001_SetPoint { get; private set; }

        //LevelController
        [DataMember] [ProcessVariable] public double LCX001_ControlOut { get; private set; }
        [DataMember] [ProcessVariable] public double LCX001_Measurement { get; private set; }

        [DataMember] [ProcessVariable] public double LCX001_SetPoint { get; private set; }

        //CustomController
        [DataMember] [ProcessVariable] public double CCX001_ControlOut { get; private set; }
        [DataMember] [ProcessVariable] public double CCX001_Input1 { get; private set; }
        [DataMember] [ProcessVariable] public double CCX001_Input2 { get; private set; }
        [DataMember] [ProcessVariable] public double CCX001_Input3 { get; private set; }


        public override void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            base.DefineDevice(builder);

            builder.RequestConfigurationExtenstion<BoilerRepositoryDetails>(data => { ConfigurationData = data; });
        }



        public string CreateMessagePayload()
        {
            _logger.LogTrace("Building payload.");
            return JsonConvert.SerializeObject(this);
        }
    }
}