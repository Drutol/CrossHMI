using System.Collections.Generic;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Interfaces.Networking
{
    public interface IRecordingBindingFactory : IBindingFactory
    {
        Dictionary<string, IConsumerBinding> GetConsumerBindingsForRepository(string repository);
    }
}
