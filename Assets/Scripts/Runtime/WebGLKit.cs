using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace WebGLKits
{
    public static class WebGLKit
    {
        public static void OpenFile(Action<string, byte[]> callback)
        {
            Library.onOpenFileCompleted = callback;
            Library.OpenFile(Library.DelegateOnOpenFileCompletedEvent);
        }

        public static void Alert(string str)
        {
            Library.Alert(str);
        }

        public static void Log(string str)
        {
            Library.Log(str);
        }

        public static string EvalJs(string jsCode)
        {
            return Library.EvalJs(jsCode);
        }

        public static void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, false);
            Library.SetResolution(width, height);
        }

        //TODO: Ctrl C , Ctrl V

        //TODO: Close Window

        //TODO: Open New Window (URL)

        //TODO: Get or Set Cookies
    }


    internal static class Library
    {
        [DllImport("__Internal")]
        internal static extern int OpenFile(OnOpenFileCompleted callback);

        internal delegate void OnOpenFileCompleted(IntPtr namePtr, IntPtr chunkPtr, int chunkSize);
        internal static Action<string, byte[]> onOpenFileCompleted;

        [MonoPInvokeCallback(typeof(OnOpenFileCompleted))]
        internal static void DelegateOnOpenFileCompletedEvent(IntPtr namePtr, IntPtr chunkPtr, int chunkSize)
        {
            string name = Marshal.PtrToStringAuto(namePtr);
            var bytes = new byte[chunkSize];
            Marshal.Copy(chunkPtr, bytes, 0, chunkSize);
            onOpenFileCompleted.Invoke(name, bytes);
            onOpenFileCompleted = null;
        }

        [DllImport("__Internal")]
        internal static extern int Alert(string str);

        [DllImport("__Internal")]
        internal static extern int Log(string str);

        [DllImport("__Internal")]
        internal static extern string EvalJs(string str);

        [DllImport("__Internal")]
        internal static extern int SetResolution(int width, int height);
    }
}