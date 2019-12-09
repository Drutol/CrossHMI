using System.Collections.Generic;

namespace CrossHMI.Interfaces.Networking
{
    public interface IAdditionalRepositoryDescriptorProvider
    {
        IReadOnlyCollection<IAdditionalRepositoryDataDescriptor> Descriptors { get; }
    }
}
