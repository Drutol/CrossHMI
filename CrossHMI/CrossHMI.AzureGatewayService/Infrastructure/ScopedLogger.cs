using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CrossHMI.AzureGatewayService.Infrastructure
{
    /// <summary>
    /// Logger which holds persistent score for its instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScopedLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        private readonly ConcurrentBag<string> _context = new ConcurrentBag<string>();

        public ScopedLogger(ILoggerFactory factory)
        {
            _logger = factory?.CreateLogger(typeof(T).ToString()) ?? throw new ArgumentNullException(nameof(factory));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            _context.Add(state.ToString());
            return null;
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        void ILogger.Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            using var scopes = new DisposableList<IDisposable>(_context.Select(_logger.BeginScope));
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        /// <summary>
        /// Helper class disposing all items inside.
        /// </summary>
        /// <typeparam name="TDisposable">Disposable.</typeparam>
        class DisposableList<TDisposable> : List<TDisposable>, IDisposable where TDisposable : IDisposable
        {
            public DisposableList(IEnumerable<TDisposable> enumerable) : base(enumerable)
            {

            }

            public void Dispose()
            {
                foreach (var item in this)
                {
                    item.Dispose();
                }
            }
        }
    }

}
