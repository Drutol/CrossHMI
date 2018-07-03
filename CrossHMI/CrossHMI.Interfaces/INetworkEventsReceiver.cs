using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrossHMI.Interfaces
{
    public interface INetworkEventsReceiver
    {
        Task Initialize();

        INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T,TRaw>(string variableName)
            where T : INetworkVariable , new ();
    }
}
