using System;
using System.Runtime.Serialization;
using CrossHMI.Interfaces;

namespace CrossHMI.Shared.Configuration
{
    /// <summary>
    /// Class with extra data about the repository.
    /// </summary>
    public class BoilerRepositoryDetails : IAdditonalRepositoryDataDescriptor
    {
        /// <inheritdoc />
        public string Repository { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Lon { get; set; }
    }
}
