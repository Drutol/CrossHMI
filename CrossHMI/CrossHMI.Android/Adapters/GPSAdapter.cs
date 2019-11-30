using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Locations;
using Android.OS;
using AoLibs.Adapters.Android.Interfaces;
using CrossHMI.Interfaces.Adapters;
using CrossHMI.Models;
using CrossHMI.Models.Enums;
using Object = Java.Lang.Object;

namespace CrossHMI.Android.Adapters
{
    public class GpsAdapter : Object, IGpsAdapter, ILocationListener
    {
        private readonly IContextProvider _contextProvider;

        private readonly LocationManager _locationManager;
        private readonly SemaphoreSlim _obtainingLocationSemaphore = new SemaphoreSlim(1);
        private DateTime _lastPositionUpdate;

        private TaskCompletionSource<LatLon> _locationCompletionSource;
        private GpsAccuracy? _reportedAccuracy;

        public GpsAdapter(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _locationManager =
                (LocationManager) _contextProvider.CurrentContext.GetSystemService(Context.LocationService);
        }

        private GpsAccuracy? ReportedAccuracy
        {
            get => _reportedAccuracy;
            set
            {
                if (value == _reportedAccuracy)
                    return;
                _reportedAccuracy = value;
                GpsAccuracyChanged?.Invoke(this, value.Value);
            }
        }

        public event EventHandler<LatLon> PositionChanged;
        public event EventHandler<GpsAccuracy> GpsAccuracyChanged;

        public GpsAccuracy LastAccuracy => ReportedAccuracy ?? GpsAccuracy.Normal;
        public LatLon LastPosition { get; private set; }

        public void Start()
        {
            _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 1, 3, this);
            _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 1, 3, this);
        }

        public void Stop()
        {
            _locationManager.RemoveUpdates(this);
        }

        public async Task<LatLon> ObtainCurrentPosition()
        {
            await _obtainingLocationSemaphore.WaitAsync();
            if (DateTime.UtcNow - _lastPositionUpdate < TimeSpan.FromMinutes(1))
            {
                _obtainingLocationSemaphore.Release();
                return LastPosition;
            }

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            _locationCompletionSource = new TaskCompletionSource<LatLon>();
            cts.Token.Register(() => _locationCompletionSource?.TrySetCanceled(), false);
            Start();
            LatLon result;
            try
            {
                result = await _locationCompletionSource.Task;
            }
            catch (TaskCanceledException)
            {
                result = null;
            }

            Stop();
            _locationCompletionSource = null;
            _obtainingLocationSemaphore.Release();

            return result;
        }

        public void OnLocationChanged(Location location)
        {
            ReportedAccuracy = location.Accuracy > 10
                ? GpsAccuracy.Bad
                : GpsAccuracy.Normal;

            _lastPositionUpdate = DateTime.UtcNow;
            LastPosition = new LatLon(location.Latitude, location.Longitude);
            PositionChanged?.Invoke(this, LastPosition);
            _locationCompletionSource?.TrySetResult(LastPosition);
        }

        public void OnProviderDisabled(string provider)
        {
            ReportedAccuracy = GpsAccuracy.None;
        }

        public void OnProviderEnabled(string provider)
        {
            ReportedAccuracy = GpsAccuracy.Bad;
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }
    }
}