using UnityEngine;
using WebGLKits;

public class Test : MonoBehaviour
{
    int width = 0;
    int height = 0;
    string jsCode = "alert(1+2)";
    string uploadFileName = "";
    byte[] uploadBytes = null;
    Texture2D uploadTexture = null;
    Vector2 scroll = Vector2.zero;

    private void Awake()
    {
        width = Screen.width;
        height = Screen.height;
    }

    private void OnGUI()
    {
        scroll = GUILayout.BeginScrollView(scroll,GUILayout.Width(300));

        // BASE
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

        // OPEN FILE
        GUILayout.BeginVertical("box");
        if (string.IsNullOrEmpty(uploadFileName))
            GUILayout.Label("Try to open a image file:");
        else
            GUILayout.Label("File: " + uploadFileName);
        if (GUILayout.Button("Open File"))
        {
            uploadFileName = "";
            uploadBytes = null;
            uploadTexture = null;
            WebGLKit.OpenFile((filename, bytes) =>
            {
                WebGLKit.Log("file name: " + filename);
                WebGLKit.Log("file size: " + bytes.Length);

                uploadFileName = filename;
                uploadBytes = bytes;
                // Do something else
            });
        }
        if (uploadFileName.EndsWith(".png")
            || uploadFileName.EndsWith(".jpg")
            || uploadFileName.EndsWith(".jpeg"))
        {
            if (uploadTexture == null)
            {
                uploadTexture = new Texture2D(0, 0);
                uploadTexture.LoadImage(uploadBytes);
                uploadBytes = null;
            }
            GUILayout.Box(uploadTexture, GUILayout.Width(266), GUILayout.Height(150));
        };
        GUILayout.EndVertical();

        // RESOLUTION
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


        // EVAL JS CODE
        GUILayout.BeginVertical("box");
        GUILayout.Label("JS Code: ");
        jsCode = GUILayout.TextArea(jsCode);
        if (GUILayout.Button("Eval JS"))
        {
            var result = WebGLKit.EvalJs(jsCode);
            WebGLKit.Log(result);
        }
        GUILayout.EndVertical();


        GUILayout.EndScrollView();
    }
}