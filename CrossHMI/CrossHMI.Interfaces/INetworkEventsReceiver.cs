using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrossHMI.Interfaces
{
    public interface INetworkEventsReceiver
    {
        event EventHandler<string> EventReceived;

        Task Initialize();
    }
}
