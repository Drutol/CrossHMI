using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;

namespace CrossHMI.Shared.Devices
{
    public class Boiler : NetworkDeviceBase
    {
        public string Identifier { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }

        [AutoMap] public double CCX001_ControlOut { get; set; }
        [AutoMap] public double CCX001_Input1 { get; set; }
        [AutoMap] public double CCX001_Input2 { get; set; }
        [AutoMap] public double CCX001_Input3 { get; set; }
        [AutoMap] public double DrumX001_LIX001_Output { get; set; }
        [AutoMap] public double FCX001_ControlOut { get; set; }
        [AutoMap] public double FCX001_Measurement { get; set; }
        [AutoMap] public double FCX001_SetPoint { get; set; }
        [AutoMap] public double PipeX001_FTX001_Output { get; set; }
        [AutoMap] public double PipeX001_ValveX001_Input { get; set; }
        [AutoMap] public double LCX001_ControlOut { get; set; }
        [AutoMap] public double LCX001_Measurement { get; set; }
        [AutoMap] public double LCX001_SetPoint { get; set; }
        [AutoMap] public double PipeX002_FTX002_Output { get; set; }
        [AutoMap] public uint Simulation_UpdateRate { get; set; }

        public override void AssignRepository(string repository)
        {
            //var data = JObject.Parse(repository);

            //Lat = data["Lat"].Value<double>();
            //Lon = data["Lon"].Value<double>();

            //Identifier = data["Id"].Value<string>();
        }
    }
}
