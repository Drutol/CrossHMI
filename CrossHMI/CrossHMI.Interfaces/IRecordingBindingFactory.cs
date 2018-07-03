using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Interfaces
{
    public interface IRecordingBindingFactory : IBindingFactory
    {
        Dictionary<string,IConsumerBinding> ConsumerBindings { get; }
    }
}
