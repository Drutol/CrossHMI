using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkVariable
    {
        string Name { get; set; }

        void Initialize(string raw);
    }
}
