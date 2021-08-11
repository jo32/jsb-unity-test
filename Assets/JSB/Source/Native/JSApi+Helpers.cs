using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace QuickJS.Native
{
    using JSValueConst = JSValue;
    using JS_BOOL = Int32;

    public partial class JSApi
    {

        public static string GetString(JSContext ctx, JSAtom atom)
        {
            var strValue = JSApi.JS_AtomToString(ctx, atom);
            var str = strValue.IsString() ? GetString(ctx, strValue) : null;
            JSApi.JS_FreeValue(ctx, strValue);
            return str;
        }

        public static string GetString(JSContext ctx, JSValue val)
        {
            size_t len;
            var pstr = JSApi.JS_ToCStringLen(ctx, out len, val);
            if (pstr == IntPtr.Zero)
            {
                return null;
            }

            try
            {
                return JSApi.GetString(ctx, pstr, len);
            }
            finally
            {
                JSApi.JS_FreeCString(ctx, pstr);
            }
        }

        public static string GetNonNullString(JSContext ctx, JSValue val)
        {
            size_t len;
            var pstr = JSApi.JS_ToCStringLen(ctx, out len, val);
            if (pstr == IntPtr.Zero)
            {
                return string.Empty;
            }

            try
            {
                return JSApi.GetString(ctx, pstr, len) ?? string.Empty;
            }
            finally
            {
                JSApi.JS_FreeCString(ctx, pstr);
            }
        }

        public static unsafe void MemoryCopy(void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy)
        {
#if JSB_COMPATIBLE
            if (sourceBytesToCopy > destinationSizeInBytes)
            {
                throw new ArgumentOutOfRangeException();
            }

            var pSource = (byte*)source;
            var pDestination = (byte*)destination;

            for (int i = 0; i < sourceBytesToCopy; ++i)
            {
                pDestination[i] = pSource[i];
            }
#else
            Buffer.MemoryCopy(source, destination, destinationSizeInBytes, sourceBytesToCopy);
#endif
        }

        public static unsafe string GetString(JSContext ctx, IntPtr ptr, int len)
        {
            if (len > 0)
            {
                var str = Marshal.PtrToStringAnsi(ptr, len);
                if (str == null)
                {
#if JSB_COMPATIBLE
                    var buffer = new byte[len];
                    Marshal.Copy(ptr, buffer, 0, len);
                    return Encoding.UTF8.GetString(buffer);
#else
                    var pointer = (byte*)(void*)ptr;
                    return Encoding.UTF8.GetString(pointer, len);
#endif
                }

                return str;
            }

            return null;
        }

        public static unsafe bool ForEachProperty(JSContext ctx, JSValue this_obj, Func<JSAtom, JSValue, bool> callback)
        {
            JSPropertyEnum* ptab;
            uint plen;
            if (JSApi.JS_GetOwnPropertyNames(ctx, out ptab, out plen, this_obj, JSGPNFlags.JS_GPN_STRING_MASK) < 0)
            {
                // failed
                return false;
            }

            var stop = false;
            for (var i = 0; i < plen; i++)
            {
                var prop = JSApi.JS_GetProperty(ctx, this_obj, ptab[i].atom);
                try
                {
                    if (callback(ptab[i].atom, prop))
                    {
                        stop = true;
                        break;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    JSApi.JS_FreeValue(ctx, prop);
                }
            }

            for (var i = 0; i < plen; i++)
            {
                JSApi.JS_FreeAtom(ctx, ptab[i].atom);
            }
            return stop;
        }
    }
}