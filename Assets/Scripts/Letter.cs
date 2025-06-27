using UnityEngine;
using System.Collections;


public class Letter : MonoBehaviour
{
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] AnimationCurve ease;
    //[HideInInspector] 
    LetterType type = LetterType.Letter;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void MoveTo(Vector3 position)
    {
        StopAllCoroutines();
        StartCoroutine(MoveAnimation(position));
    }

    public void InstaMoveTo(Vector3 position)
    {
        transform.localPosition = position;
    }

    IEnumerator MoveAnimation(Vector3 to)
    {
        float t = 0;
        Vector3 from = transform.localPosition;

        while (t < moveTime)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(
                from,
                to,
                ease.Evaluate(t / moveTime)
            );

            yield return null;
        }

        transform.localPosition = to;
    }

    public bool IsSpace
    {
        get => type == LetterType.Space;
    }

    public bool IsLinebreak
    {
        get => type == LetterType.Linebreak;
    }

    public LetterType Type
    {
        set => type = value;
    }

    public Vector3 Position
    {
        get => transform.localPosition;
    }

    public Color Color
    {
        set => sr.color = value;
    }
}
