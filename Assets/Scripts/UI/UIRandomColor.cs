using UnityEngine;

public class UIRandomColor : MonoBehaviour
{
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            RandomizeColors();
        }
    }

    public void RandomizeColors()
    {
        UILetter.RandomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        cam.backgroundColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GameEvents.RandomizeColors.Invoke();
    }
}
