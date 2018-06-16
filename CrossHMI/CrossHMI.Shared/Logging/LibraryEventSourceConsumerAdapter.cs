using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.Linq;
using System.Text;
using CrossHMI.Interfaces;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.Diagnostics.EventFlow.Configuration;
using Microsoft.Diagnostics.EventFlow.Inputs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace CrossHMI.Shared.Logging
{
    public class LibraryEventSourceConsumerAdapter : ILibraryEventSourceConsumerAdapter
    {
        private readonly ILibraryCompositionProvider _libraryCompositionProvider;
        private DiagnosticPipeline _pipeline;

        public LibraryEventSourceConsumerAdapter(ILibraryCompositionProvider libraryCompositionProvider)
        {
            _libraryCompositionProvider = libraryCompositionProvider;
        }

        public void StartListening()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                //inputs
                ["inputs:0:type"] = "EventSource",
                ["inputs:0:sources:0:providerName"] = "MyEventSource",
                ["inputs:0:sources:0:level"] = "Informational",
                //["inputs:1:type"]= "Trace",
                //["inputs:1:traceLevel"] = "Warning",
                //outputs
                ["outputs:0:type"] = "StdOutput",
            });



            _pipeline = DiagnosticPipelineFactory.CreatePipeline(configBuilder.Build());

            MyEventSource.Log.Message("resultTest");         
        }

        [EventSource(Name = "MyEventSource")]
        private class MyEventSource : EventSource
        {
            public static readonly MyEventSource Log = new MyEventSource();

            [Event(1, Level = EventLevel.Informational, Message = "{0}")]
            public void Message(string message)
            {
                WriteEvent(1, message);
            }
        }
    }
}
