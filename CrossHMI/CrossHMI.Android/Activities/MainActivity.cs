using System;
using System.Collections.Generic;
using System.Diagnostics;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using AoLibs.Adapters.Android;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.Core.PageProviders;
using Autofac;
using CrossHMI.Android.Fragment;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Models.Enums;
using CrossHMI.Shared.Statics;
using CrossHMI.Shared.ViewModels;
using Newtonsoft.Json;

namespace CrossHMI.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : AppCompatActivity, IOnActivityResultProvider, IOnNewIntentProvider
    {
        private MainViewModel _viewModel;
        private ILogAdapter<MainActivity> _logger;

        private EventHandler<Intent> _activityNewIntentEventHandler;
        private EventHandler<(int RequestCode, Result ResultCode, Intent Data)> _activityResultEventHandler;

        public MainActivity()
        {
            _logger = ResourceLocator.GetLogger<MainActivity>();
            Instance = this;
        }

        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            var pageDefinitions = new Dictionary<PageIndex, IPageProvider<NavigationFragmentBase>>
            {
                //cached
                {PageIndex.DashboardPage, new CachedPageProvider<DashboardPageFragment>()},
                {PageIndex.BoilderDetailsPage, new CachedPageProvider<BoilerDetailsPageFragment>()},
                {PageIndex.GenericDeviceDetailsPage, new CachedPageProvider<GenericDevicePageFragment>()}
            };

            var manager = new NavigationManager<PageIndex>(SupportFragmentManager, RootView, pageDefinitions, new ViewModelResolver());
            App.Current.NavigationManager = manager;
            _logger.LogDebug("Created navigation manager.");
            using (var scope = ResourceLocator.ObtainScope())
            {
                var messageBoxProvider = scope.Resolve<IMessageBoxProvider>() as MessageBoxProvider;
                messageBoxProvider.ShowLoadingPopupRequest += MessageBoxProviderOnShowLoadingPopupRequest;
                messageBoxProvider.HideLoadingPopupRequest += MessageBoxProviderOnHideLoadingPopupRequest;
            }

            RequestPermissions(new[] {Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation},
                12);
            _logger.LogDebug("Requested location permissions.");
            _viewModel = ViewModelLocator.MainViewModel;
            _viewModel.Initialized();
        }

        public override void OnBackPressed()
        {
            if (!App.Current.NavigationManager.OnBackRequested())
            {
                MoveTaskToBack(true);
            }
        }

        private void MessageBoxProviderOnHideLoadingPopupRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MessageBoxProviderOnShowLoadingPopupRequest(object sender, (string title, string content) e)
        {
            throw new NotImplementedException();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            _activityResultEventHandler?.Invoke(this, (requestCode, resultCode, data));
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            _activityNewIntentEventHandler.Invoke(this, intent);
        }

        event EventHandler<(int RequestCode, Result ResultCode, Intent Data)>
            IOnActivityEvent<(int RequestCode, Result ResultCode, Intent Data)>.Received
            {
                add => _activityResultEventHandler += value;
                remove => _activityResultEventHandler -= value;
            }

        event EventHandler<Intent> IOnActivityEvent<Intent>.Received
        {
            add => _activityNewIntentEventHandler += value;
            remove => _activityNewIntentEventHandler -= value;
        }

        #region Views

        private FrameLayout _rootView;

        public FrameLayout RootView => _rootView ?? (_rootView = FindViewById<FrameLayout>(Resource.Id.RootView));

        #endregion


        private class ViewModelResolver : IDependencyResolver
        {
            public TViewModel Resolve<TViewModel>()
            {
                Log.Debug(nameof(App), $"Resolving ViewModel: {typeof(TViewModel).Name}");
                try
                {
                    using (var scope = ResourceLocator.ObtainScope())
                    {
                        return scope.Resolve<TViewModel>();
                    }
                }
                catch (Exception e)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }
    }
}