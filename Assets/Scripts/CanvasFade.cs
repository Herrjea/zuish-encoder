using UnityEngine;
using System.Collections;


public class CanvasFade : MonoBehaviour
{
    [SerializeField] float fadeDuration = .5f;
    [SerializeField] AnimationCurve ease;

    bool visible = true;
    CanvasGroup cg;


    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }


    public void ToggleVisibility()
    {
        StopAllCoroutines();

        if (visible)
            StartCoroutine(Fade(false));
        else
            StartCoroutine(Fade(true));
    }

    IEnumerator Fade(bool fadeIn)
    {
        float t = 0;
        visible = fadeIn;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = fadeIn ? ease.Evaluate(t / fadeDuration) : 1 - ease.Evaluate(t / fadeDuration);

            yield return null;
        }

        cg.alpha = fadeIn ? 1 : 0;


        print(visible);
    }

    public void InstaHide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public void InstaShow()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        cg.alpha = visible ? 1 : 0;
    }

    public bool activeInHierarchy
    {
        get => gameObject.activeInHierarchy;
    }
}
