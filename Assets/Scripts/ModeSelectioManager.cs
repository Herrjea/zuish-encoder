using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;


public class ModeSelectioManager : MonoBehaviour
{
    [SerializeField] Image loadCursor;
    [SerializeField] Image typeCursor;

    [SerializeField] List<MenuElement> elements;
    [SerializeField] List<string> sceneNames;
    int current = 0;

    [SerializeField] UIRandomColor randomColor;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UploadTextFile();
#endif


    void Awake()
    {
        elements[current].Play();

        GameEvents.RandomizeColors.AddListener(RandomizeColors);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            elements[current].Stop();
            current = (current + 1) % elements.Count;
            elements[current].Play();

            randomColor.RandomizeColors();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            elements[current].Stop();
            current = (current - 1 + elements.Count) % elements.Count;
            elements[current].Play();

            randomColor.RandomizeColors();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (current == 0)
                UploadTextFile();
            else
                SceneManager.LoadScene(sceneNames[current]);
#else
            SceneManager.LoadScene(sceneNames[current]);
#endif
        }
    }


    void RandomizeColors()
    {
        loadCursor.color = UILetter.RandomColor;
        typeCursor.color = UILetter.RandomColor;
    }


    public void OnFileSelected(string content)
    {
        PlayerPrefs.SetString("file", content);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneNames[0]);
    }
}
