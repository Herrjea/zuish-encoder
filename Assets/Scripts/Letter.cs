using UnityEngine;
using System.Collections;


public class Letter : MonoBehaviour
{
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] AnimationCurve ease;
    [HideInInspector] public bool isSpace = false;

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

            if (isSpace)
            print(t);

            yield return null;
        }

        transform.position = to;
    }
}
