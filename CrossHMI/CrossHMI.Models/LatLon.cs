using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Models
{
    public class LatLon
    {
        public LatLon(double latitude, double longitude)
        {
            Lat = latitude;
            Lon = longitude;
        }

        public double Lat { get; }
        public double Lon { get; }
    }
}
