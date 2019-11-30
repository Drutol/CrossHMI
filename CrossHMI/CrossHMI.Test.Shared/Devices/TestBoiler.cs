using System.Collections.Generic;
using System.ComponentModel;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using CrossHMI.Shared.Devices;

namespace CrossHMI.Test.Shared.Devices
{
    public class TestBoiler : NetworkDeviceBaseWithConfiguration<BoilersConfigurationData>
    {
        public TestBoiler()
        {
            PropertyChanged += OnPropertyChanged;
        }

        [ProcessVariable] public double CCX001_ControlOut { get; private set; }
        [ProcessVariable(AutoDefine = false)] public double CCX001_Input1 { get; private set; }

        [ProcessVariable(RaisePropertyChanged = false)]
        public double CCX001_Input2 { get; private set; }

        public BoilerRepositoryDetails AssignedExtension { get; set; }
        public HashSet<string> UpdatedProperties { get; set; } = new HashSet<string>();
        public HashSet<string> ChangedProperties { get; set; } = new HashSet<string>();

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ChangedProperties.Add(e.PropertyName);
        }

        public override void AssignRepository(string repository)
        {
        }

        public override void DefineDevice(INetworkDeviceDefinitionBuilder<BoilersConfigurationData> builder)
        {
            base.DefineDevice(builder);

            builder.DefineVariable<double>("CCX001_Input3");

            builder.DefineConfigurationExtenstion(config => config.RepositoriesDetails, ExtenstionAssigned);
        }

        public override void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            base.ProcessPropertyUpdate(variableName, value);

            UpdatedProperties.Add(variableName);
        }

        private void ExtenstionAssigned(BoilerRepositoryDetails obj)
        {
            AssignedExtension = obj;
        }
    }
}