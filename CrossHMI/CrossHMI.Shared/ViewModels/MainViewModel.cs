using AoLibs.Navigation.Core.Interfaces;
using CrossHMI.Models.Enums;
using GalaSoft.MvvmLight;

namespace CrossHMI.Shared.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;

        public MainViewModel(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void Initialized()
        {
            _navigationManager.Navigate(PageIndex.DashboardPage);
        }
    }
}