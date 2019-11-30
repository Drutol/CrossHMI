﻿using CrossHMI.Interfaces.Networking;

namespace CrossHMI.Interfaces
{
    public delegate void NetworkVariableUpdateEventHandler<T>(INetworkVariableUpdateSource<T> deviceUpdateSourceBase,
        T value);
}