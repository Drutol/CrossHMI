using System.IO;
using CrossHMI.Interfaces.Adapters;

namespace CrossHMI.Test.Shared.Helpers
{
    public class ConfigurationResourcesProvider : IConfigurationResourcesProvider
    {
        public Stream ObtainLibraryConfiguration()
        {
            return File.Open("Assets/TestLibraryConfiguration.xml", FileMode.Open, FileAccess.Read);
        }
    }
}