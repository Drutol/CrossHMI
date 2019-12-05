using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces.Networking
{
    public interface IAdditionalRepositoryDescriptorProvider
    {
        IReadOnlyCollection<IAdditionalRepositoryDataDescriptor> Descriptors { get; }
    }
}
