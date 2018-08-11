using System.ComponentModel;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;

namespace CrossHMI.Shared.Devices
{
    /// <summary>
    /// Model class representing the boiler repository. This class can be treated as ViewModel with implemented <see cref="INotifyPropertyChanged"/> events.
    /// </summary>
    public class Boiler : NetworkDeviceBaseWithConfiguration<BoilersConfigurationData>
    {
        /// <summary>
        /// Gets the repository of the device.
        /// </summary>
        public string Repository { get; private set; }

        /// <summary>
        /// Gets or
        /// </summary>
        public double Lat { get; private set; }
        public double Lon { get; private set; }

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
        //Metadata
        //[ProcessVariable] public uint Simulation_UpdateRate { get; set; }

        /// <inheritdoc />
        public override void AssignRepository(string repository)
        {
            Repository = repository;
        }

        /// <inheritdoc />
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
