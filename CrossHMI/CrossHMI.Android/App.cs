using System;
using System.Net;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using AoLibs.Adapters.Android;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using Autofac;
using CrossHMI.Android.Adapters;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Models.Enums;
using CrossHMI.Shared.Statics;

namespace CrossHMI.Android
{
    [Application]
    public class App : Application
    {
        public App(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
            Current = this;
        }

        public INavigationManager<PageIndex> NavigationManager { get; set; }

        public static App Current { get; private set; }

        public override void OnCreate()
        {
            Log.Debug(nameof(App), "Starting application.");
            Log.Debug(nameof(App), "Starting dependencies registration.");
            AppInitializationRoutines.Init(AdaptersRegistration);
            Log.Debug(nameof(App), "Finished registering dependencies.");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Log.Debug(nameof(App), "Finished application start. Commencing Activity launch.");
            base.OnCreate();
        }

        private void AdaptersRegistration(ContainerBuilder containerBuilder)
        {
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

            containerBuilder.RegisterType<ConfigurationResourcesProvider>().As<IConfigurationResourcesProvider>()
                .SingleInstance();

            containerBuilder.Register(ctx => NavigationManager).As<INavigationManager<PageIndex>>();
            containerBuilder.Register(ctx => MainActivity.Instance).As<IOnActivityResultProvider>()
                .As<IOnNewIntentProvider>();

            containerBuilder.RegisterGeneric(typeof(LogAdapter<>)).As(typeof(ILogAdapter<>));
        }

        private class ContextProvider : IContextProvider
        {
            public Activity CurrentActivity => MainActivity.Instance;

            public Context CurrentContext => MainActivity.Instance;
        }
    }
}