using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CrossHMI.Interfaces;
using UAOOI.Configuration.Networking;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.MessageHandling;

namespace CrossHMI.Shared.BL
{
    public class NetworkEventsReceiver : DataManagementSetup, INetworkEventsReceiver
    {
        public event EventHandler<string> EventReceived;

        public NetworkEventsReceiver(IBindingFactory bindingFactory, IConfigurationFactory configurationFactory,
            IMessageHandlerFactory messageHandlerFactory, IEncodingFactory encodingFactory)
        {
            BindingFactory = bindingFactory;
            ConfigurationFactory = configurationFactory;
            MessageHandlerFactory = messageHandlerFactory;
            EncodingFactory = encodingFactory;
        }

        public async Task Initialize()
        {
            await Task.Run(() => { Start(); });
        }
    }
}
