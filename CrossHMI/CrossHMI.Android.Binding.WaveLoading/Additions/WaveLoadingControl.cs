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

namespace ME.Itangqi.Waveloadingview
{
    public partial class WaveLoadingControl
    {
        static IntPtr id_onDraw_Landroid_graphics_Canvas_;
        // Metadata.xml XPath method reference: path="/api/package[@name='me.itangqi.waveloadingview']/class[@name='WaveLoadingView']/method[@name='onDraw' and count(parameter)=1 and parameter[1][@type='android.graphics.Canvas']]"
        [Register("onDraw", "(Landroid/graphics/Canvas;)V", "GetOnDraw_Landroid_graphics_Canvas_Handler")]
        protected override unsafe void OnDraw(global::Android.Graphics.Canvas p0)
        {
            if (id_onDraw_Landroid_graphics_Canvas_ == IntPtr.Zero)
                id_onDraw_Landroid_graphics_Canvas_ = JNIEnv.GetMethodID(class_ref, "onDraw", "(Landroid/graphics/Canvas;)V");
            try
            {
                JValue* __args = stackalloc JValue[1];
                __args[0] = new JValue(p0);

                if (((object)this).GetType() == ThresholdType)
                    JNIEnv.CallVoidMethod(((global::Java.Lang.Object)this).Handle, id_onDraw_Landroid_graphics_Canvas_, __args);
                else
                    JNIEnv.CallNonvirtualVoidMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "onDraw", "(Landroid/graphics/Canvas;)V"), __args);
            }
            finally
            {
            }
        }
    }
}