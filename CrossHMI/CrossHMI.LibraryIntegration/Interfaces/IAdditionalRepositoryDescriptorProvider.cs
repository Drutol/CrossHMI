using System.Collections.Generic;

namespace CrossHMI.LibraryIntegration.Interfaces
{
    /// <summary>
    /// Component providing all additional <see cref="IAdditionalRepositoryDataDescriptor"/> found in configuration file.
    /// </summary>
    public interface IAdditionalRepositoryDescriptorProvider
    {
        IReadOnlyCollection<IAdditionalRepositoryDataDescriptor> Descriptors { get; }
    }
}
