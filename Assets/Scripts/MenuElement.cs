using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuElement : MonoBehaviour
{
    [SerializeField] float frequency = 1;
    [SerializeField] float amplitude = 10;
    [SerializeField] float waviness = 1;

    List<RectTransform> letters;
    GameObject cursor;

    void Awake()
    {
        LoadLetters();

        cursor = transform.parent.Find("Cursor").gameObject;
        cursor.SetActive(false);
    }

    public void Play()
    {
        StopAllCoroutines();
        cursor.SetActive(true);
        StartCoroutine(Animation());
    }

    void LoadLetters()
    {
        letters = new List<RectTransform>();
        foreach (Transform t in transform)
            letters.Add(t.GetComponent<RectTransform>());
    }

    IEnumerator Animation()
    {
        while (true)
        {
            if (letters[0] == null)
                LoadLetters();

            foreach (RectTransform letter in letters)
                letter.anchoredPosition = new Vector2(
                    amplitude * Mathf.Cos(Time.time * frequency + letter.anchoredPosition.y * waviness / 100),
                    letter.anchoredPosition.y
                );

            yield return null;
        }
    }

    public void Stop()
    {
        StopAllCoroutines();

        foreach (RectTransform letter in letters)
            letter.anchoredPosition = new Vector2(
                0,
                letter.anchoredPosition.y
            );

        cursor.SetActive(false);
    }
}
