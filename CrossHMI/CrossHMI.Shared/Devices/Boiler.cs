using CrossHMI.Interfaces;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;

namespace CrossHMI.Shared.Devices
{
    public class Boiler : ViewModelBase, INetworkDevice
    {
        private int _value;
        private bool _toggle;
        public string Identifier { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }

        public bool Toggle
        {
            get => _toggle;
            set
            {
                _toggle = value;
                RaisePropertyChanged();
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public void AssignRepository(string repository)
        {
            var data = JObject.Parse(repository);

            Lat = data["Lat"].Value<double>();
            Lon = data["Lon"].Value<double>();

            Identifier = data["Id"].Value<string>();
        }

        public void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            switch (value)
            {
                case bool b:
                    if (variableName == "BoolToggle")
                        Toggle = b;
                    break;
                case int i:
                    if (variableName == "ValueInt32")
                        Value = i;
                    break;
            }
        }

        public void DefineVariables(INetworkDeviceDefinitionBuilder builder)
        {
            builder.Define<bool>("BoolToggle")
                .Define<int>("ValueInt32");
        }
    }
}
