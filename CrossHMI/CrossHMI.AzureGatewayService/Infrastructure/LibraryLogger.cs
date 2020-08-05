using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using CrossHMI.AzureGatewayService.Interfaces;
using Microsoft.Extensions.Logging;
using UAOOI.Networking.Core;

namespace CrossHMI.AzureGatewayService.Infrastructure
{
    /// <summary>
    /// Converts EventSource logs into ILogger logs.
    /// </summary>
    public class LibraryLogger : EventListener, ILibraryLogger
    {
        private readonly Dictionary<EventSource, ILogger> _loggers = new Dictionary<EventSource, ILogger>();

        public LibraryLogger(
            ILoggerFactory loggerFactory,
            IEnumerable<INetworkingEventSourceProvider> providers)
        {
            foreach (var networkingEventSourceProvider in providers)
            {
                var source = networkingEventSourceProvider.GetPartEventSource();
                _loggers[source] = loggerFactory.CreateLogger(source.GetType());
                EnableEvents(source, EventLevel.LogAlways);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            _loggers[eventData.EventSource].Log(eventData.Level switch
            {
                EventLevel.Verbose => LogLevel.Trace,
                EventLevel.Critical => LogLevel.Critical,
                EventLevel.Error => LogLevel.Error,
                EventLevel.Informational => LogLevel.Information,
                EventLevel.LogAlways => LogLevel.Trace,
                EventLevel.Warning => LogLevel.Warning,
                _ => throw new ArgumentOutOfRangeException()
            }, eventData.EventId, eventData.Message, eventData.Payload.ToArray());
        }
    }
}
