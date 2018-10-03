using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.NavArgs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CrossHMI.Shared.ViewModels
{
    public class BoilerDetailsViewModel : ViewModelBase
    {
        private readonly IUriLauncherAdapter _uriLauncherAdapter;

        private Boiler _boiler;

        public BoilerDetailsViewModel(IUriLauncherAdapter uriLauncherAdapter)
        {
            _uriLauncherAdapter = uriLauncherAdapter;
        }

        public void NavigatedTo(BoilderDetailsNavArgs navArgs)
        {
            Boiler = navArgs.Boiler;
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
            _uriLauncherAdapter.LaunchUri(
                new Uri($"http://maps.google.com/maps?&daddr={Boiler.Lat},{Boiler.Lon}"));
        });
    }
}
