using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UILetter : MonoBehaviour
{
    Image image;
    RectTransform rt;

    public static Color RandomColor = Color.black;


    void Awake()
    {
        Init();

        GameEvents.RandomizeColors.AddListener(GetRandomColor);
    }

    public void Init()
    {
        image = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    void GetRandomColor()
    {
        Color = RandomColor;
    }

    public Sprite Sprite
    {
        set => image.sprite = value;
    }

    public Color Color
    {
        set => image.color = value;
    }

    public int Size
    {
        set => rt.sizeDelta = new Vector2(value, value);
    }

    public float Position
    {
        set
        {
            rt.anchoredPosition = new Vector2(value, 0);
        }
    }
}
