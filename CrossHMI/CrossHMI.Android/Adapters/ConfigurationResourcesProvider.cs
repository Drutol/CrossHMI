using System.IO;
using Android.Content.Res;
using AoLibs.Adapters.Android.Interfaces;
using CrossHMI.Interfaces.Adapters;

namespace CrossHMI.Android.Adapters
{
    public class ConfigurationResourcesProvider : IConfigurationResourcesProvider
    {
        private readonly IContextProvider _contextProvider;
        private readonly ILogAdapter<ConfigurationResourcesProvider> _logger;

        public ConfigurationResourcesProvider(IContextProvider contextProvider,
            ILogAdapter<ConfigurationResourcesProvider> logger)
        {
            _contextProvider = contextProvider;
            _logger = logger;
        }

        public Stream ObtainLibraryConfigurationXML()
        {
            _logger.LogDebug("Reading configuration data from native asset data file on Android.");
            return _contextProvider.CurrentContext.Assets.Open("LibraryConfiguration.xml", Access.Random);
        }
    }
}