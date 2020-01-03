using System.IO;
using Android.Content.Res;
using AoLibs.Adapters.Android.Interfaces;
using CrossHMI.Interfaces.Adapters;
using Microsoft.Extensions.Logging;

namespace CrossHMI.Android.Adapters
{
    public class ConfigurationResourcesProvider : IConfigurationResourcesProvider
    {
        private readonly IContextProvider _contextProvider;
        private readonly ILogger<ConfigurationResourcesProvider> _logger;

        public ConfigurationResourcesProvider(IContextProvider contextProvider,
            ILogger<ConfigurationResourcesProvider> logger)
        {
            _contextProvider = contextProvider;
            _logger = logger;
        }

        public Stream ObtainLibraryConfiguration()
        {
            _logger.LogDebug("Reading configuration data from native asset data file on Android.");
            return _contextProvider.CurrentContext.Assets.Open("LibraryConfiguration.json", Access.Random);
        }
    }
}