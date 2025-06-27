using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class ModeSelectioManager : MonoBehaviour
{
    [SerializeField] Image loadCursor;
    [SerializeField] Image typeCursor;

    [SerializeField] List<MenuElement> elements;
    [SerializeField] List<string> sceneNames;
    int current = 0;

    [SerializeField] UIRandomColor randomColor;


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
            SceneManager.LoadScene(sceneNames[current]);
        }
    }


    void RandomizeColors()
    {
        loadCursor.color = UILetter.RandomColor;
        typeCursor.color = UILetter.RandomColor;
    }
}
