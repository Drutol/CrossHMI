using AoLibs.Navigation.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Models.Enums;
using GalaSoft.MvvmLight;

namespace CrossHMI.Shared.ViewModels
{
    /// <summary>
    /// MainViewModel not being tied with any page. Acts as an always present ViewModel in background.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;

        /// <summary>
        /// Creates new <see cref="MainViewModel"/> intance.
        /// </summary>
        /// <param name="navigationManager"></param>
        public MainViewModel(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Called upon initialization of the platform the application is currently running on.
        /// </summary>
        public void Initialized()
        {
            _navigationManager.Navigate(PageIndex.DashboardPage);
        }
    }
}