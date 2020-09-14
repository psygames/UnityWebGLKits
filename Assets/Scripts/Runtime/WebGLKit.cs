using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace WebGLKits
{
    public static class WebGLKit
    {
        public static void UploadFile(Action<string, byte[]> callback)
        {
            Library.onUploadFileCompleted = callback;
            Library.UploadFile(Library.DelegateOnUploadFileCompletedEvent);
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
        internal static extern int UploadFile(OnUploadFileCompleted callback);

        internal delegate void OnUploadFileCompleted(IntPtr namePtr, IntPtr chunkPtr, int chunkSize);
        internal static Action<string, byte[]> onUploadFileCompleted;

        [MonoPInvokeCallback(typeof(OnUploadFileCompleted))]
        internal static void DelegateOnUploadFileCompletedEvent(IntPtr namePtr, IntPtr chunkPtr, int chunkSize)
        {
            string name = Marshal.PtrToStringAuto(namePtr);
            var bytes = new byte[chunkSize];
            Marshal.Copy(chunkPtr, bytes, 0, chunkSize);
            onUploadFileCompleted.Invoke(name, bytes);
            onUploadFileCompleted = null;
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