using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkVariableUpdateSource<T>
    {
        event EventHandler<T> Updated;
    }
}
