using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Interfaces;
using CrossHMI.Interfaces.Adapters;

namespace CrossHMI.Android.Adapters
{
    public class ConfigurationResourcesProvider : IConfigurationResourcesProvider
    {
        private readonly IContextProvider _contextProvider;

        public ConfigurationResourcesProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public Stream ObtainLibraryConfigurationXML()
        {
            return _contextProvider.CurrentContext.Assets.Open("LibraryConfiguration.xml", Access.Random);
        }
    }
}