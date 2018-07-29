using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Models;
using CrossHMI.Shared.Devices;
using GalaSoft.MvvmLight;
using UAOOI.Networking.UDPMessageHandler.Diagnostic;

namespace CrossHMI.Shared.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private const string Repository1 = "BoilersArea_Boiler #1";
        private const string Repository2 = "BoilersArea_Boiler #2";
        private const string Repository3 = "BoilersArea_Boiler #3";
        private const string Repository4 = "BoilersArea_Boiler #4";

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
            _updateSources.Add(_networkEventsReceiver.ObtainEventSourceForDevice<Boiler>(Repository3));
            _updateSources.Add(_networkEventsReceiver.ObtainEventSourceForDevice<Boiler>(Repository4));

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