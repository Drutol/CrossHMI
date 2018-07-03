using System.Collections.ObjectModel;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Models;
using CrossHMI.Shared.Variables;
using GalaSoft.MvvmLight;

namespace CrossHMI.Shared.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly INetworkEventsReceiver _networkEventsReceiver;
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private ObservableCollection<VariableUpdatedEntry> _updates;
        private INetworkVariableUpdateSource<VariableUpdatedEntry> _massTestEventSource;

        public DashboardViewModel(INetworkEventsReceiver networkEventsReceiver, IDispatcherAdapter dispatcherAdapter)
        {
            _networkEventsReceiver = networkEventsReceiver;
            _dispatcherAdapter = dispatcherAdapter;
            Updates = new ObservableCollection<VariableUpdatedEntry>();

            Initialize();
        }

        private async void Initialize()
        {
            await _networkEventsReceiver.Initialize();
            _massTestEventSource = _networkEventsReceiver.ObtainEventSourceForVariable<VariableUpdatedEntry,uint>("MassTest_99");
            _massTestEventSource.Updated += MassTestEventSourceOnUpdated;
        }

        private void MassTestEventSourceOnUpdated(object sender, VariableUpdatedEntry e)
        {
            _dispatcherAdapter.Run(() =>
            {
                Updates.Add(e);
            });         
        }

        public ObservableCollection<VariableUpdatedEntry> Updates
        {
            get => _updates;
            set
            {
                _updates = value;
                RaisePropertyChanged();
            }
        }
    }
}