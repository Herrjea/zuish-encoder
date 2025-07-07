using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    [HideInInspector] public float currentHue = 1, currentSat = 0, currentVal = 0;
    [SerializeField] CanvasFade canvas;
    [SerializeField] Encoder encoder;
    [SerializeField] bool canGoBack = true;
    [SerializeField] TypingCursor cursor;
    Camera cam;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    //private static extern void DownloadFile(string filename, string base64Data);
    private static extern void DownloadFile(byte[] array, int byteLength, string fileName);
#endif


    void Awake()
    {
        cam = Camera.main;

        GameEvents.NewBGColor.AddListener(UpdateBGColor);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TakeScreenshot();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.ToggleVisibility();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            encoder.ToggleSpacing();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            encoder.LongerLine();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            encoder.ShorterLine();
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            RandomizeColors();
        }

        if (canGoBack && Input.GetKeyDown(KeyCode.Backspace))
        {
            LoadModeSelectionScene();
        }
    }

    void TakeScreenshot()
    {
        StartCoroutine(GetScreenshotBytes());
    }

    private IEnumerator GetScreenshotBytes()
    {
        bool canvasWasActive = canvas.activeInHierarchy;
        bool cursorWasActive = cursor.activeInHierarchy;

        if (canvasWasActive)
            canvas.InstaHide();
        if (cursorWasActive)
            cursor.Hide();

        if (canvasWasActive || cursorWasActive)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();


#if UNITY_WEBGL && !UNITY_EDITOR
        int width = Screen.width;
        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        byte[] pngData = tex.EncodeToPNG();
        Destroy(tex);    

        DownloadFile(
            pngData, 
            pngData.Length, 
            "screenshot " + DateTime.Now.ToString("yy-MM-dd HH-mm-ss") + ".png"
        );
#endif

#if UNITY_EDITOR
        ScreenCapture.CaptureScreenshot(
            //Application.persistentDataPath + "/Screenshots/screenshot " +
                "Assets/Resources/Screenshots/screenshot " +
                DateTime.Now.ToString("yy-MM-dd HH-mm-ss") + ".png",
            1
        );
#endif
        

        yield return null;
        if (canvasWasActive)
            canvas.InstaShow();
        if (cursorWasActive)
            cursor.Show();

        print("screenshot taken");
    }


    void UpdateBGColor(Color color)
    {
        cam.backgroundColor = color;
    }

    public void RandomizeColors()
    {
        GameEvents.RandomizeColors.Invoke();
    }

    public void LoadModeSelectionScene()
    {
        SceneManager.LoadScene("ModeSelection");
    }
}
