using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    [HideInInspector] public float currentHue = 1, currentSat = 0, currentVal = 0;
    [SerializeField] GameObject canvas;
    [SerializeField] Encoder encoder;
    Camera cam;

    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string base64Data);


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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvas.SetActive(!canvas.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
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

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("ModeSelection");
        }
    }

    public void TakeScreenshot()
    {
        StartCoroutine(ScreenshotCoroutine());
        StartCoroutine(GetScreenshotBytes());
    }

    IEnumerator ScreenshotCoroutine()
    {
        bool canvasWasActive = canvas.activeInHierarchy;

        if (canvasWasActive)
        {
            canvas.SetActive(false);
            yield return null;
        }

        ScreenCapture.CaptureScreenshot(
            //Application.persistentDataPath + "/Screenshots/screenshot " +
                "Assets/Resources/Screenshots/screenshot " +
                DateTime.Now.ToString("yy-MM-dd HH-mm-ss") + ".png",
            1
        );

        yield return null;
        canvas.SetActive(canvasWasActive);

        print("screenshot taken");
    }

    private IEnumerator GetScreenshotBytes()
    {
        yield return new WaitForEndOfFrame();

        /*
        im saving a screenshot in unity, but i want to upload the webgl build of my project to itch.io, and the user can't access the files there. how can i make it to that when the user takes a screenshot they are asked where in their machine they want to store it?

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        byte[] imageBytes = screenImage.EncodeToPNG();
        Destroy(screenImage);

        string base64Image = System.Convert.ToBase64String(imageBytes);
        DownloadFile("screenshot.png", base64Image);
        */
    }


    void UpdateBGColor(Color color)
    {
        cam.backgroundColor = color;
    }

    public void RandomizeColors()
    {
        GameEvents.RandomizeColors.Invoke();
    }
}
