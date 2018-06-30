using System;
using System.Diagnostics;
using System.Net;
using Android.App;
using Android.Runtime;
using AoLibs.Adapters.Android;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using Autofac;
using CrossHMI.Android.Adapters;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Models.Enums;
using CrossHMI.Shared.Statics;
using NavigationLib.Android.Navigation;

namespace CrossHMI.Android
{
    [Application]
    public class App : Application
    {
        public App(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
            Current = this;
        }

        public LifecycleInfoProvider LifetimeInfoProvider { get; private set; }
        public INavigationManager<PageIndex> NavigationManager { get; set; }

        public static App Current { get; private set; }

        public override void OnCreate()
        {
            AppInitializationRoutines.Init(AdaptersRegistration);
            NavigationFragmentBase.ViewModelResolver = new ViewModelResolver();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            base.OnCreate();
        }

        private void AdaptersRegistration(ContainerBuilder containerBuilder)
        {
            LifetimeInfoProvider = new LifecycleInfoProvider();

            containerBuilder.RegisterType<ClipboardProvider>().As<IClipboardProvider>().SingleInstance();
            containerBuilder.RegisterType<DispatcherAdapter>().As<IDispatcherAdapter>().SingleInstance();
            containerBuilder.RegisterType<FileStorageProvider>().As<IFileStorageProvider>().SingleInstance();
            containerBuilder.RegisterType<MessageBoxProvider>().As<IMessageBoxProvider>().SingleInstance();
            containerBuilder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();
            containerBuilder.RegisterType<UriLauncherAdapter>().As<IUriLauncherAdapter>().SingleInstance();
            containerBuilder.RegisterType<VersionProvider>().As<IVersionProvider>().SingleInstance();
            containerBuilder.RegisterType<PickerAdapter>().As<IPickerAdapter>().SingleInstance();
            containerBuilder.RegisterType<ContextProvider>().As<IContextProvider>().SingleInstance();
            containerBuilder.RegisterType<PhotoPickerAdapter>().As<IPhotoPickerAdapter>().SingleInstance();
            containerBuilder.RegisterType<PhoneCallAdapter>().As<IPhoneCallAdapter>().SingleInstance();

            containerBuilder.RegisterType<ConfigurationResourcesProvider>().As<IConfigurationResourcesProvider>().SingleInstance();

            containerBuilder.RegisterInstance(LifetimeInfoProvider).As<ILifecycleInfoProvider>();

            containerBuilder.Register(ctx => NavigationManager).As<INavigationManager<PageIndex>>();
            containerBuilder.Register(ctx => MainActivity.Instance).As<IOnActivityResultProvider>()
                .As<IOnNewIntentProvider>();
        }


        private class ViewModelResolver : IViewModelResolver
        {
            public TViewModel Resolve<TViewModel>()
            {
                try
                {
                    using (var scope = ViewModelLocator.ObtainScope())
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

        private class ContextProvider : IContextProvider
        {
            public Activity CurrentContext => MainActivity.Instance;
        }
    }
}