using System;
using System.Collections.Generic;
using System.Text;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.NavArgs;
using GalaSoft.MvvmLight;

namespace CrossHMI.Shared.ViewModels
{
    public class BoilderDetailsViewModel : ViewModelBase
    {
        private Boiler _boiler;

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
    }
}
