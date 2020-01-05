using System.Collections.Generic;
using System.ComponentModel;
using CrossHMI.LibraryIntegration.Infrastructure.Devices;
using CrossHMI.LibraryIntegration.Interfaces;
using CrossHMI.Shared.Infrastructure.Configuration;

namespace CrossHMI.Test.Shared.Devices
{
    public class TestBoiler : NetworkDeviceBase
    {
        public override string Repository { get; set; }

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

        public override void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            base.DefineDevice(builder);

            builder.DefineVariable<double>("CCX001_Input3");

            builder.RequestConfigurationExtenstion<BoilerRepositoryDetails>(ExtenstionAssigned);
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