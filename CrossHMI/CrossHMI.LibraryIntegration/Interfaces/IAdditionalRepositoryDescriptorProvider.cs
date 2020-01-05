using System.Collections.Generic;

namespace CrossHMI.LibraryIntegration.Interfaces
{
    public interface IAdditionalRepositoryDescriptorProvider
    {
        IReadOnlyCollection<IAdditionalRepositoryDataDescriptor> Descriptors { get; }
    }
}
