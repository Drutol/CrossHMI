using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Extensions.Logging;

namespace CrossHMI.Android.Adapters
{
    public class AndroidLoggerProvider : ILoggerProvider
    {
        private static readonly Dictionary<LogLevel, Action<string, string>> LogMethods =
            new Dictionary<LogLevel, Action<string, string>>();

        static AndroidLoggerProvider()
        {
            foreach (LogLevel logLevel in Enum.GetValues(typeof(LogLevel)))
            {
                Action<string, string> logMethod;
                switch (logLevel)
                {
                    case LogLevel.Trace:
                        logMethod = (tag, message) => global::Android.Util.Log.Verbose(tag, message);
                        break;
                    case LogLevel.Debug:
                        logMethod = (tag, message) => global::Android.Util.Log.Debug(tag, message);
                        break;
                    case LogLevel.Information:
                        logMethod = (tag, message) => global::Android.Util.Log.Info(tag, message);
                        break;
                    case LogLevel.Warning:
                        logMethod = (tag, message) => global::Android.Util.Log.Warn(tag, message);
                        break;
                    case LogLevel.Error:
                        logMethod = (tag, message) => global::Android.Util.Log.Error(tag, message);
                        break;
                    case LogLevel.Critical:
                        logMethod = (tag, message) => global::Android.Util.Log.Wtf(tag, message);
                        break;
                    case LogLevel.None:
                        logMethod = (tag, message) => global::Android.Util.Log.Debug(tag, message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                LogMethods[logLevel] = logMethod;
            }
        }

        public void Dispose()
        {

        }

        public ILogger CreateLogger(string categoryName)
        {
            return new SystemLogger(categoryName, this);
        }

        class SystemLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly AndroidLoggerProvider _parent;
            private readonly List<ScopeLifetime> _scopes = new List<ScopeLifetime>();

            public SystemLogger(string categoryName, AndroidLoggerProvider parent)
            {
                _categoryName = categoryName;
                _parent = parent;
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                var message = formatter(state, exception);
                if (_scopes.Any())
                    message += $" [Scopes: {string.Join(", ", _scopes.Select(lifetime => lifetime.Scope))}]";
                LogMethods[logLevel](_categoryName, message);
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                var scope = new ScopeLifetime(state.ToString(), this);
                _scopes.Add(scope);
                return scope;
            }

            class ScopeLifetime : IDisposable
            {
                private readonly SystemLogger _parent;

                public string Scope { get; }

                public ScopeLifetime(string scope, SystemLogger parent)
                {
                    _parent = parent;
                    Scope = scope;
                }

                public void Dispose()
                {
                    _parent._scopes.Remove(this);
                }
            }
        }
    }
}