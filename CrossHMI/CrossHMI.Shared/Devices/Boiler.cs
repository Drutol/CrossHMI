using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using CrossHMI.Shared.Statics;

namespace CrossHMI.Shared.Devices
{
    /// <summary>
    ///     Model class representing the boiler repository. This class can be treated as ViewModel with implemented
    ///     <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public class Boiler : NetworkDeviceBase
    {
        private readonly Dictionary<string, bool> _thresholdExceeded = new Dictionary<string, bool>();
        private bool _isAnyValueThresholdExeeded;
        private readonly ILogAdapter<Boiler> _logger;
        private Dictionary<string, double> _thresholds = new Dictionary<string, double>();
        private string _repository;

        public Boiler()
        {
            try
            {
                _logger = ResourceLocator.GetLogger<Boiler>();
            }
            catch
            {
                //logger unavailable in current configuration
            }
        }

        /// <summary>
        ///     Gets the repository of the device.
        /// </summary>
        public override string Repository
        {
            get => _repository;
            set
            {
                _repository = value;
                _logger?.LogDebug($"Assigned repository: {value}");
            }
        }

        /// <summary>
        ///     Gets or sets Latitude.
        /// </summary>
        public double Lat { get; private set; }

        /// <summary>
        ///     Gets or sets Longitude.
        /// </summary>
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


        public bool IsAnyValueThresholdExeeded
        {
            get => _isAnyValueThresholdExeeded;
            set
            {
                _isAnyValueThresholdExeeded = value;
                RaisePropertyChanged();
            }
        }

        public string Notes { get; set; }

        /// <summary>
        ///     Fired whenever value of given property exceeds or gets back to desired level again.
        /// </summary>
        public event EventHandler<(string Property, bool ExceedsThreshold)> PropertyThresholdStatusChanged;

        public override void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            base.ProcessPropertyUpdate(variableName, value);

            if (!_thresholds.ContainsKey(variableName))
                return;

            if (value is double d)
            {
                var previousState = _thresholdExceeded[variableName];
                _thresholdExceeded[variableName] = d > _thresholds[variableName];

                if (previousState != _thresholdExceeded[variableName])
                    PropertyThresholdStatusChanged?.Invoke(this, (variableName, _thresholdExceeded[variableName]));
            }

            IsAnyValueThresholdExeeded = _thresholdExceeded.Any(pair => pair.Value);
        }

        /// <inheritdoc />
        public override void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            _logger?.LogDebug("Defining device.");
            base.DefineDevice(builder);

            builder.DefineConfigurationExtenstion(
                data => (data as BoilersConfigurationData).RepositoriesDetails,
                ExtenstionAssigned);
        }

        private void ExtenstionAssigned(BoilerRepositoryDetails extension)
        {
            _logger?.LogDebug("Configuration extension has been assigned.");
            Lat = extension.Lat;
            Lon = extension.Lon;
            Notes = extension.Notes;

            _thresholds = extension.ValueThresholds;
            foreach (var threshold in _thresholds) _thresholdExceeded.Add(threshold.Key, false);
        }
    }
}