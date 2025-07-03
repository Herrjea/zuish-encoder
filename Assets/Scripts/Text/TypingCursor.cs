using UnityEngine;
using System.Collections;

public class TypingCursor : MonoBehaviour
{
    [SerializeField] float animationDuration = 1;
    [SerializeField] AnimationCurve ease;
    SpriteRenderer sr;
    Color originalColor, transparent;
    bool hidden = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        SetColor(sr.color);
    }

    public void Play()
    {
        hidden = false;
        StartCoroutine(Animation());
    }

    public void Restart()
    {
        t = 0;
    }

    float t;
    IEnumerator Animation()
    {
        while (true)
        {
            t = 0;

            while (t < animationDuration)
            {
                if (hidden)
                {
                    sr.color = transparent;
                }
                else
                {
                    t += Time.deltaTime;
                    sr.color = Color.Lerp(
                        originalColor,
                        transparent,
                        ease.Evaluate(t / animationDuration)
                    );
                }
                
                yield return null;
            }

            yield return null;
        }
    }

    public void Hide()
    {
        hidden = true;
    }

    public void Show()
    {
        hidden = false;
    }


    public void SetColor(Color color)
    {
        originalColor = color;
        transparent = originalColor;
        transparent.a = 0;
    }

    public bool activeInHierarchy
    {
        get => gameObject.activeInHierarchy;
    }
}
