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

    IEnumerator MoveAnimation(Vector3 to)
    {
        float t = 0;
        Vector3 from = transform.position;

        while (t < moveTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(
                from,
                to,
                ease.Evaluate(t / moveTime)
            );

            yield return null;
        }

        transform.position = to;
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
}
