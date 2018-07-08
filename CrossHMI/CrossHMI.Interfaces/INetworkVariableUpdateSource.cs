using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkVariableUpdateSource<T>
    {
        string Name { get; }

        event NetworkVariableUpdateEventHandler<T> Updated;
    }
}
