using System.Runtime.CompilerServices;
using AoLibs.Navigation.Core.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Models.Enums;
using GalaSoft.MvvmLight;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("CrossHMI.Test.Shared")]

namespace CrossHMI.Shared.ViewModels
{
    /// <summary>
    ///     MainViewModel not being tied with any page. Acts as an always present ViewModel in background.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly INavigationManager<PageIndex> _navigationManager;

        /// <summary>
        ///     Creates new <see cref="MainViewModel" /> intance.
        /// </summary>
        /// <param name="navigationManager"></param>
        public MainViewModel(
            INavigationManager<PageIndex> navigationManager,
            ILogger<MainViewModel> logger)
        {
            _navigationManager = navigationManager;
            _logger = logger;
        }

        /// <summary>
        ///     Called upon initialization of the platform the application is currently running on.
        /// </summary>
        public void Initialized()
        {
            _logger.LogDebug("Initializing MainViewModel, performing first navigation.");
            _navigationManager.Navigate(PageIndex.DashboardPage);
        }
    }
}