﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkDeviceEventSource<T>
    {
        event EventHandler<T> NewDeviceEvent;
    }
}