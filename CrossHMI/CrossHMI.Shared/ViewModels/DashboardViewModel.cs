using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using CrossHMI.LibraryIntegration.Infrastructure.Devices;
using CrossHMI.LibraryIntegration.Interfaces;
using CrossHMI.Models.Enums;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.Infrastructure.Configuration;
using CrossHMI.Shared.NavArgs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.Logging;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.ViewModels
{
    /// <summary>
    ///     ViewModel class for dashboard page presenting a list of all boilers defined.
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private const string Repository1 = "BoilersArea_Boiler #1";
        private const string Repository2 = "BoilersArea_Boiler #2";
        private const string Repository3 = "BoilersArea_Boiler #3";
        private const string Repository4 = "BoilersArea_Boiler #4";
        private readonly ILogger<DashboardViewModel> _logger;
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly Func<Boiler> _boilerFactory;

        private readonly IDispatcherAdapter _dispatcherAdapter;
        private readonly INetworkEventsManager _networkEventsManager;

        private readonly List<INetworkDeviceUpdateSource<NetworkDeviceBase>> _updateSources =
            new List<INetworkDeviceUpdateSource<NetworkDeviceBase>>();

        private ObservableCollection<NetworkDeviceBase> _boilers;

        /// <summary>
        ///     Creates new instance of <see cref="DashboardViewModel" />
        /// </summary>
        /// <param name="networkEventsManager">Network events receiver.</param>
        /// <param name="navigationManager"></param>
        /// <param name="_boilerFactory"></param>
        /// <param name="logger"></param>
        public DashboardViewModel(
            IDispatcherAdapter dispatcherAdapter,
            INetworkEventsManager networkEventsManager,
            INavigationManager<PageIndex> navigationManager,
            Func<Boiler> _boilerFactory,
            ILogger<DashboardViewModel> logger)
        {
            _dispatcherAdapter = dispatcherAdapter;
            _networkEventsManager = networkEventsManager;
            _navigationManager = navigationManager;
            this._boilerFactory = _boilerFactory;
            _logger = logger;
            Initialize();
        }

        /// <summary>
        ///     Gets the collection of all boilers.
        /// </summary>
        public ObservableCollection<NetworkDeviceBase> Boilers
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
            _logger.LogDebug($"Navigating to boiler details page: {boiler.Repository}");
            _navigationManager.Navigate(PageIndex.BoilderDetailsPage, new BoilderDetailsNavArgs {Boiler = boiler});
        });

        public RelayCommand<GenericDevice> NavigateToGenericDeviceDetailsCommand => new RelayCommand<GenericDevice>(
            device =>
            {
                _logger.LogDebug($"Navigating to boiler details page: {device.Repository}");
                _navigationManager.Navigate(PageIndex.GenericDeviceDetailsPage,
                    new GenericDetailsNavArgs {GenericDevice = device});
            });

        private async void Initialize()
        {
            _logger.LogDebug("Initializing network events manager.");
            await _networkEventsManager.Initialize();

            _logger.LogDebug("Network events manager initialized. Creating event sources for repositories.");
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository1, _boilerFactory));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository2, _boilerFactory));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository3, _boilerFactory));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice<Boiler>(Repository4, _boilerFactory));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice(Repository1,
                () => new GenericDevice(
                    _dispatcherAdapter,
                    new GenericDeviceConfiguration
                {
                    Properties = new Dictionary<string, BuiltInType>
                    {
                        {"DrumX001_LIX001_Output", BuiltInType.Double},
                        {"PipeX002_FTX002_Output", BuiltInType.Double}
                    }
                })));
            _updateSources.Add(_networkEventsManager.ObtainEventSourceForDevice(Repository2,
                () => new GenericDevice(
                    _dispatcherAdapter,
                    new GenericDeviceConfiguration
                {
                    Properties = new Dictionary<string, BuiltInType>
                    {
                        {"FCX001_Measurement", BuiltInType.Double},
                        {"LCX001_Measurement", BuiltInType.Double}
                    }
                })));

            _logger.LogDebug("Event sources created successfully.");
            Boilers = new ObservableCollection<NetworkDeviceBase>(_updateSources.Select(source => source.Device));
        }
    }
}