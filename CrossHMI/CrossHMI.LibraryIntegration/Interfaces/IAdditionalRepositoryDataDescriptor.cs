using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.LibraryIntegration.Interfaces
{
    /// <summary>
    ///     Interface for objects that are found in extended <see cref="ConfigurationData" />.
    /// </summary>
    public interface IAdditionalRepositoryDataDescriptor
    {
        /// <summary>
        ///     The repository that this additional data is targeting.
        /// </summary>
        string Repository { get; }
    }
}