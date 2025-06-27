using UnityEngine;
using System.Collections;

public class TypingCursor : MonoBehaviour
{
    [SerializeField] float animationDuration = 1;
    [SerializeField] AnimationCurve ease;
    SpriteRenderer sr;
    Color originalColor, transparent;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        SetColor(sr.color);
    }

    public void Play()
    {
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
                t += Time.deltaTime;
                sr.color = Color.Lerp(
                    originalColor,
                    transparent,
                    ease.Evaluate(t / animationDuration)
                );
                
                yield return null;
            }

            yield return null;
        }
    }


    public void SetColor(Color color)
    {
        originalColor = color;
        transparent = originalColor;
        transparent.a = 0;
    }
}
