using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces.Adapters
{
    public interface ILogAdapter<T>
    {
        void LogDebug(string message);
    }
}
