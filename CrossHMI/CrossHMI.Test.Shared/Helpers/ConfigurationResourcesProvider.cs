using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CrossHMI.Interfaces.Adapters;

namespace CrossHMI.Test.Shared.Helpers
{
    public class ConfigurationResourcesProvider : IConfigurationResourcesProvider
    {
        public Stream ObtainLibraryConfigurationXML()
        {
            return File.Open("Assets/TestLibraryConfiguration.xml", FileMode.Open, FileAccess.Read);
        }
    }
}
