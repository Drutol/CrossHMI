using System;
using System.Runtime.Serialization;
using CrossHMI.Interfaces;

namespace CrossHMI.Shared.Configuration
{
    public class BoilerRepositoryDetails : IAdditonalRepositoryDataDescriptor
    {
        public string Repository { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
