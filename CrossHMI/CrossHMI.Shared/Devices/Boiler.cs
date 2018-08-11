using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;

namespace CrossHMI.Shared.Devices
{
    public class Boiler : NetworkDeviceBaseWithConfiguration<BoilersConfigurationData>
    {
        public string Repository { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }

        //InputPipe
        [ProcessVariable] public double PipeX001_FTX001_Output { get; set; }
        [ProcessVariable] public double PipeX001_ValveX001_Input { get; set; }
        //Drum
        [ProcessVariable] public double DrumX001_LIX001_Output { get; set; }
        //OutputPipe
        [ProcessVariable] public double PipeX002_FTX002_Output { get; set; }
        //FlowController
        [ProcessVariable] public double FCX001_ControlOut { get; set; }
        [ProcessVariable] public double FCX001_Measurement { get; set; }
        [ProcessVariable] public double FCX001_SetPoint { get; set; }
        //LevelController
        [ProcessVariable] public double LCX001_ControlOut { get; set; }
        [ProcessVariable] public double LCX001_Measurement { get; set; }
        [ProcessVariable] public double LCX001_SetPoint { get; set; }
        //CustomController
        [ProcessVariable] public double CCX001_ControlOut { get; set; }
        [ProcessVariable] public double CCX001_Input1 { get; set; }
        [ProcessVariable] public double CCX001_Input2 { get; set; }
        [ProcessVariable] public double CCX001_Input3 { get; set; }
        //Metadata
        //[ProcessVariable] public uint Simulation_UpdateRate { get; set; }

        public override void AssignRepository(string repository)
        {
            Repository = repository;
        }

        public override void DefineDevice(INetworkDeviceDefinitionBuilder<BoilersConfigurationData> builder)
        {
            base.DefineDevice(builder);

            builder.DefineConfigurationExtenstion(
                data => data.RepositoriesDetails,
                ExtenstionAssigned);
        }

        private void ExtenstionAssigned(BoilerRepositoryDetails extension)
        {
            Lat = extension.Lat;
            Lon = extension.Lon;
        }
    }
}
