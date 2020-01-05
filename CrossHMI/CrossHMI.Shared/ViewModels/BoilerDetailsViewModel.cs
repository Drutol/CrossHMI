using System;
using System.Windows.Input;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.NavArgs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.Logging;

namespace CrossHMI.Shared.ViewModels
{
    public class BoilerDetailsViewModel : ViewModelBase
    {
        private readonly ILogger<BoilerDetailsViewModel> _logger;
        private readonly IUriLauncherAdapter _uriLauncherAdapter;

        private Boiler _boiler;

        public BoilerDetailsViewModel(IUriLauncherAdapter uriLauncherAdapter,
            ILogger<BoilerDetailsViewModel> logger)
        {
            _uriLauncherAdapter = uriLauncherAdapter;
            _logger = logger;
        }

        public Boiler Boiler
        {
            get => _boiler;
            set
            {
                _boiler = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LaunchNavigationCommand => new RelayCommand(() =>
        {
            _logger.LogDebug("Launching external navigation.");
            _uriLauncherAdapter.LaunchUri(
                new Uri($"http://maps.google.com/maps?&daddr={Boiler.Lat},{Boiler.Lon}"));
        });

        public void NavigatedTo(BoilderDetailsNavArgs navArgs)
        {
            Boiler = navArgs.Boiler;
        }
    }
}