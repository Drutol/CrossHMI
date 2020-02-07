using System.ComponentModel;
using CrossHMI.AzureGatewayService.Infrastructure.Configuration;
using CrossHMI.AzureGatewayService.Interfaces;
using CrossHMI.LibraryIntegration.Infrastructure.Devices;
using CrossHMI.LibraryIntegration.Interfaces;
using Newtonsoft.Json;

namespace CrossHMI.AzureGatewayService.Devices
{
    /// <summary>
    ///     Model class representing the boiler repository. This class can be treated as ViewModel with implemented
    ///     <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public class Boiler : NetworkDeviceBase, IAzureEnabledNetworkDevice
    {
        [JsonIgnore]
        public override string Repository { get; set; }

        [JsonIgnore]
        public BoilerRepositoryDetails ConfigurationData { get; private set; }

        [JsonIgnore] public IAzureConnectionParameters AzureConnectionParameters => ConfigurationData;

        //InputPipe
        [ProcessVariable] public double PipeX001_FTX001_Output { get; private set; }

        [ProcessVariable] public double PipeX001_ValveX001_Input { get; private set; }

        //Drum
        [ProcessVariable] public double DrumX001_LIX001_Output { get; private set; }

        //OutputPipe
        [ProcessVariable] public double PipeX002_FTX002_Output { get; private set; }

        //FlowController
        [ProcessVariable] public double FCX001_ControlOut { get; private set; }
        [ProcessVariable] public double FCX001_Measurement { get; private set; }

        [ProcessVariable] public double FCX001_SetPoint { get; private set; }

        //LevelController
        [ProcessVariable] public double LCX001_ControlOut { get; private set; }
        [ProcessVariable] public double LCX001_Measurement { get; private set; }

        [ProcessVariable] public double LCX001_SetPoint { get; private set; }

        //CustomController
        [ProcessVariable] public double CCX001_ControlOut { get; private set; }
        [ProcessVariable] public double CCX001_Input1 { get; private set; }
        [ProcessVariable] public double CCX001_Input2 { get; private set; }
        [ProcessVariable] public double CCX001_Input3 { get; private set; }


        public override void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            base.DefineDevice(builder);

            builder.RequestConfigurationExtenstion<BoilerRepositoryDetails>(data => { ConfigurationData = data; });
        }

        public string CreateMessagePayload()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}