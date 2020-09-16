using System;
using System.IO;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace WebGLKits
{
    public static class WebGLKit
    {
        public static void OpenFile(Action<string, byte[]> callback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Library.onOpenFileCompleted = callback;
            Library.OpenFile(Library.DelegateOnOpenFileCompletedEvent);
#elif UNITY_EDITOR
            string filePath = UnityEditor.EditorUtility.OpenFilePanel("OpenFile", "", "*.*");
            if (!string.IsNullOrEmpty(filePath))
            {
                callback(Path.GetFileName(filePath), File.ReadAllBytes(filePath));
            }
#else
            Debug.LogError("the function not support on this platform.");
#endif
        }

        public static void Alert(string str)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Library.Alert(str);
#elif UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog("alert", str, "OK");
#else
            Debug.LogError("the function not support on this platform.");
#endif
        }

        public static void Log(string str)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Library.Log(str);
#else
            Debug.Log(str);
#endif
        }

        public static string EvalJs(string jsCode)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return Library.EvalJs(jsCode);
#else
            Debug.LogError("the function not support on this platform.");
            return "";
#endif
        }

        public static void SetResolution(int width, int height)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Screen.SetResolution(width, height, false);
            Library.SetResolution(width, height);
#else
            Screen.SetResolution(width, height, false);
#endif
        }

        //TODO: IME Input Support

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