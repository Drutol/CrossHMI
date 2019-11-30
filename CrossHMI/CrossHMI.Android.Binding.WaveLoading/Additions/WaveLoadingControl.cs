using System;
using Android.Graphics;
using Android.Runtime;
using Object = Java.Lang.Object;

namespace ME.Itangqi.Waveloadingview
{
    public partial class WaveLoadingControl
    {
        private static IntPtr id_onDraw_Landroid_graphics_Canvas_;

        // Metadata.xml XPath method reference: path="/api/package[@name='me.itangqi.waveloadingview']/class[@name='WaveLoadingView']/method[@name='onDraw' and count(parameter)=1 and parameter[1][@type='android.graphics.Canvas']]"
        [Register("onDraw", "(Landroid/graphics/Canvas;)V", "GetOnDraw_Landroid_graphics_Canvas_Handler")]
        protected override unsafe void OnDraw(Canvas p0)
        {
            if (id_onDraw_Landroid_graphics_Canvas_ == IntPtr.Zero)
                id_onDraw_Landroid_graphics_Canvas_ =
                    JNIEnv.GetMethodID(class_ref, "onDraw", "(Landroid/graphics/Canvas;)V");
            var __args = stackalloc JValue[1];
            __args[0] = new JValue(p0);

            if (GetType() == ThresholdType)
                JNIEnv.CallVoidMethod(((Object) this).Handle, id_onDraw_Landroid_graphics_Canvas_, __args);
            else
                JNIEnv.CallNonvirtualVoidMethod(((Object) this).Handle, ThresholdClass,
                    JNIEnv.GetMethodID(ThresholdClass, "onDraw", "(Landroid/graphics/Canvas;)V"), __args);
        }
    }
}