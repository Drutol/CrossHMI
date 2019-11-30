using CrossHMI.Shared.Devices;
using CrossHMI.Shared.NavArgs;
using GalaSoft.MvvmLight;

namespace CrossHMI.Shared.ViewModels
{
    public class GenericDetailsViewModel : ViewModelBase
    {
        private GenericDevice _device;

        public GenericDevice Device
        {
            get => _device;
            set
            {
                _device = value;
                RaisePropertyChanged();
            }
        }

        public void NavigatedTo(GenericDetailsNavArgs genericDetailsNavArgs)
        {
            Device = genericDetailsNavArgs.GenericDevice;
        }
    }
}