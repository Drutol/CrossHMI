using System.Collections.Generic;
using CrossHMI.LibraryIntegration.Interfaces;

namespace CrossHMI.Shared.Infrastructure.Configuration
{
    /// <summary>
    ///     Class with extra data about the repository.
    /// </summary>
    public class BoilerRepositoryDetails : IAdditionalRepositoryDataDescriptor
    {
        /// <summary>
        ///     Gets or sets the latitude.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        ///     Gets or sets the longitude.
        /// </summary>
        public double Lon { get; set; }

        /// <summary>
        ///     Gets or sets value thresholds.
        /// </summary>
        public Dictionary<string, double> ValueThresholds { get; set; }

        /// <summary>
        ///     Notes attached to given boiler.
        /// </summary>
        public string Notes { get; set; }

        /// <inheritdoc />
        public string Repository { get; set; }
    }
}