using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CrossHMI.Interfaces.Adapters;

namespace CrossHMI.Android.Adapters
{
    public class LogAdapter<T> : ILogAdapter<T>
    {
        private string _tag;

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