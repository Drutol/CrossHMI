﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CrossHMI.Interfaces.Adapters
{
    /// <summary>
    /// Inteface for providing configuration contents from underlying system of assets.
    /// </summary>
    public interface IConfigurationResourcesProvider
    {
        /// <summary>
        /// Obtains a stream pointing to raw configuration XML.
        /// </summary>
        /// <returns></returns>
        Stream ObtainLibraryConfigurationXML();
    }
}