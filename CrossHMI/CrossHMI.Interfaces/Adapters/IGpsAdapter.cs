using System;
using System.Threading.Tasks;
using CrossHMI.Models;
using CrossHMI.Models.Enums;

namespace CrossHMI.Interfaces.Adapters
{
    public interface IGpsAdapter
    {
        LatLon LastPosition { get; }
        GpsAccuracy LastAccuracy { get; }

        void Start();
        void Stop();

        event EventHandler<LatLon> PositionChanged;
        event EventHandler<GpsAccuracy> GpsAccuracyChanged;

        Task<LatLon> ObtainCurrentPosition();
    }
}