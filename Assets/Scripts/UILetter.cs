using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UILetter : MonoBehaviour
{
    Image image;
    RectTransform rt;
    TextDirection direction = TextDirection.Vertical;

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
            if (direction == TextDirection.Vertical)
                rt.anchoredPosition = new Vector2(0, -value);
            else
                rt.anchoredPosition = new Vector2(value, 0);
        }
    }

    public TextDirection Direction
    {
        set
        {
            direction = value;
            float position;

            if (direction == TextDirection.Vertical)
            {
                position = rt.anchoredPosition.x;

                rt.pivot = new Vector2(0.5f, 1);
                rt.anchorMin = new Vector2(0.5f, 1);
                rt.anchorMax = new Vector2(0.5f, 1);
            }
            else
            {
                position = -rt.anchoredPosition.y;

                rt.pivot = new Vector2(0, 0.5f);
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(0, 0.5f);
            }
            
            Position = position;
        }
    }
}
