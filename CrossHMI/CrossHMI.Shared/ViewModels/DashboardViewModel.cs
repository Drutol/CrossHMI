using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Models;
using CrossHMI.Models.Enums;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.NavArgs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UAOOI.Networking.UDPMessageHandler.Diagnostic;

namespace CrossHMI.Shared.ViewModels
{
    /// <summary>
    /// ViewModel class for dashboard page presenting a list of all boilers defined.
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private const string Repository1 = "BoilersArea_Boiler #1";
        private const string Repository2 = "BoilersArea_Boiler #2";
        private const string Repository3 = "BoilersArea_Boiler #3";
        private const string Repository4 = "BoilersArea_Boiler #4";

        private readonly INetworkEventsManager _networkEventsManager;
        private readonly INavigationManager<PageIndex> _navigationManager;
        private ObservableCollection<Boiler> _boilers;

        private readonly List<INetworkDeviceUpdateSource<Boiler>> _updateSources =
            new List<INetworkDeviceUpdateSource<Boiler>>();

        /// <summary>
        /// Creates new instance of <see cref="DashboardViewModel"/>
        /// </summary>
        /// <param name="networkEventsManager">Network events receiver.</param>
        /// <param name="navigationManager"></param>
        public DashboardViewModel(INetworkEventsManager networkEventsManager,
            INavigationManager<PageIndex> navigationManager)
        {
            _networkEventsManager = networkEventsManager;
            _navigationManager = navigationManager;
            Initialize();
        }

        private async void Initialize()
        {
            await _networkEventsManager.Initialize();

            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository1));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository2));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository3));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository4));

            Boilers = new ObservableCollection<Boiler>(_updateSources.Select(source => source.Device));
        }

        /// <summary>
        /// Gets the collection of all boilers.
        /// </summary>
        public ObservableCollection<Boiler> Boilers
        {
            get => _boilers;
            set
            {
                _boilers = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<Boiler> NavigateToBoilerDetailsCommand => new RelayCommand<Boiler>(boiler =>
        {
            _navigationManager.Navigate(PageIndex.BoilderDetailsPage, new BoilderDetailsNavArgs {Boiler = boiler});
        });
    }
}