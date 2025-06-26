using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;


public class Manager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string base64Data);


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(TakeScreenshot());
            StartCoroutine(GetScreenshotBytes());
        }
    }

    IEnumerator TakeScreenshot()
    {
        canvas.SetActive(false);
        yield return null;

        ScreenCapture.CaptureScreenshot(
            //Application.persistentDataPath + "/Screenshots/screenshot " +
                "Assets/Resources/Screenshots/screenshot " +
                DateTime.Now.ToString("yy-MM-dd HH-mm-ss") + ".png",
            1
        );

        yield return null;
        canvas.SetActive(true);

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
}
