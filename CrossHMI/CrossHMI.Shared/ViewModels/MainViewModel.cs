using AoLibs.Navigation.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Models.Enums;
using GalaSoft.MvvmLight;

namespace CrossHMI.Shared.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly INetworkEventsReceiver _eventsReceiver;

        public MainViewModel(INavigationManager<PageIndex> navigationManager, INetworkEventsReceiver eventsReceiver)
        {
            _navigationManager = navigationManager;
            _eventsReceiver = eventsReceiver;
        }

        public void Initialized()
        {
            _navigationManager.Navigate(PageIndex.DashboardPage);
        }
    }
}