﻿using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;

namespace CrossHMI.Shared.Devices
{
    public class Boiler : NetworkDeviceBase
    {
        public string Identifier { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }

        [ProcessVariable] public double CCX001_ControlOut { get; set; }
        [ProcessVariable] public double CCX001_Input1 { get; set; }
        [ProcessVariable] public double CCX001_Input2 { get; set; }
        [ProcessVariable] public double CCX001_Input3 { get; set; }
        [ProcessVariable] public double DrumX001_LIX001_Output { get; set; }
        [ProcessVariable] public double FCX001_ControlOut { get; set; }
        [ProcessVariable] public double FCX001_Measurement { get; set; }
        [ProcessVariable] public double FCX001_SetPoint { get; set; }
        [ProcessVariable] public double PipeX001_FTX001_Output { get; set; }
        [ProcessVariable] public double PipeX001_ValveX001_Input { get; set; }
        [ProcessVariable] public double LCX001_ControlOut { get; set; }
        [ProcessVariable] public double LCX001_Measurement { get; set; }
        [ProcessVariable] public double LCX001_SetPoint { get; set; }
        [ProcessVariable] public double PipeX002_FTX002_Output { get; set; }
        [ProcessVariable] public uint Simulation_UpdateRate { get; set; }

        public override void AssignRepository(string repository)
        {

        }

        public override void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            base.DefineDevice(builder);

            builder.DefineConfigurationExtenstion<BoilersConfigurationData, BoilerRepositoryDetails>(
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
