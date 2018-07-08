using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Models;
using CrossHMI.Shared.Devices;
using GalaSoft.MvvmLight;
using UAOOI.Networking.UDPMessageHandler.Diagnostic;

namespace CrossHMI.Shared.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private const string Repository1 = "{\"Id\": \"Boiler1\",\"Lat\": 55.68399,\"Lon\": 16.29569}";
        private const string Repository2 = "{\"Id\": \"Boiler2\",\"Lat\": 45.68399,\"Lon\": 22.29569}";

        private readonly INetworkEventsReceiver _networkEventsReceiver;
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private ObservableCollection<Boiler> _boilers;
        private readonly List<INetworkDeviceUpdateSource<Boiler>> _updateSources = new List<INetworkDeviceUpdateSource<Boiler>>();

        public DashboardViewModel(INetworkEventsReceiver networkEventsReceiver, IDispatcherAdapter dispatcherAdapter)
        {
            _networkEventsReceiver = networkEventsReceiver;
            _dispatcherAdapter = dispatcherAdapter;
            Initialize();
        }

        private async void Initialize()
        {
            await _networkEventsReceiver.Initialize();

            _updateSources.Add(_networkEventsReceiver.ObtainEventSourceForDevice<Boiler>(Repository1));
            _updateSources.Add(_networkEventsReceiver.ObtainEventSourceForDevice<Boiler>(Repository2));

            Boilers = new ObservableCollection<Boiler>(_updateSources.Select(source => source.Device));
        }

        public ObservableCollection<Boiler> Boilers
        {
            get => _boilers;
            set
            {
                _boilers = value;
                RaisePropertyChanged();
            }
        }
    }
}