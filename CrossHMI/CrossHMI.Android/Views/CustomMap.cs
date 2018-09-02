using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CrossHMI.Android.Views
{
    public class CustomMap : MapView
    {
        protected CustomMap(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CustomMap(Context context) : base(context)
        {
        }

        public CustomMap(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomMap(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        public CustomMap(Context context, GoogleMapOptions options) : base(context, options)
        {
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            Parent.RequestDisallowInterceptTouchEvent(true);
            return base.DispatchTouchEvent(e);
        }
    }
}