using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CrossHMI.Interfaces.Adapters
{
    public interface IConfigurationResourcesProvider
    {
        Stream ObtainLibraryConfigurationXML();
    }
}
