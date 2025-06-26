using UnityEngine;
using System;
using System.Collections;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(TakeScreenshot());
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
}
