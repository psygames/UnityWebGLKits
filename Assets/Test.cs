using System;
using System.IO;
using System.Text;
using UnityEngine;
using WebGLKits;

public class Test : MonoBehaviour
{
    int width = 0;
    int height = 0;
    string jsCode = "alert(1+2)";

    private void Awake()
    {
        width = Screen.width;
        height = Screen.height;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Base:");
        if (GUILayout.Button("console.log"))
        {
            WebGLKit.Log("test log");
        }

        if (GUILayout.Button("alert"))
        {
            WebGLKit.Alert("test alert");
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        if (GUILayout.Button("UploadFile"))
        {
            WebGLKit.UploadFile((filename, bytes) =>
            {
                WebGLKit.Log("file name: " + filename);
                WebGLKit.Log("file size: " + bytes.Length);

                // Do something else
            });
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Resolution: ");
        GUILayout.BeginHorizontal();
        width = int.Parse(GUILayout.TextField(width.ToString()));
        GUILayout.Label(" x ");
        height = int.Parse(GUILayout.TextField(height.ToString()));
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Set Resolution"))
        {
            WebGLKit.SetResolution(width, height);
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("JS Code: ");
        jsCode = GUILayout.TextArea(jsCode);
        if (GUILayout.Button("Eval JS"))
        {
            var result = WebGLKit.EvalJs(jsCode);
            WebGLKit.Log(result);
        }
        GUILayout.EndVertical();
    }
}
