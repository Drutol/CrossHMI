using Android.Util;
using CrossHMI.Interfaces.Adapters;

namespace CrossHMI.Android.Adapters
{
    public class LogAdapter<T> : ILogAdapter<T>
    {
        private readonly string _tag;

        public LogAdapter()
        {
            _tag = $"CrossHMI-{typeof(T).Name}";
        }

        public void LogDebug(string message)
        {
            Log.Debug(_tag, message);
        }
    }
}