using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] float positionSmoothTime = 1f;
    [SerializeField] float sizeSmoothTime = 1f;
    [SerializeField] bool fixedSize = false;
    [SerializeField] float fixedSizeSize = 11;
    Vector3 positionVelocity = Vector3.zero;
    float sizeVelocity = 0;

    Vector3 targetPosition = Vector3.zero;
    float targetSize = 5;

    Camera cam;
    Vector3 offset;


    void Awake()
    {
        GameEvents.NewTextCenter.AddListener(OnNewPosition);
        GameEvents.NewTextSize.AddListener(OnNewSize);

        cam = GetComponent<Camera>();
        offset = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition + offset,
            ref positionVelocity,
            positionSmoothTime
        );

        if (fixedSize)
            cam.orthographicSize = fixedSizeSize;
        else
            cam.orthographicSize = Mathf.SmoothDamp(
                cam.orthographicSize,
                targetSize,
                ref sizeVelocity,
                sizeSmoothTime
            );
    }


    void OnNewPosition(Vector3 position)
    {
        targetPosition = position;
    }

    void OnNewSize(float x, float y)
    {
        // adjust sizes
        x /= 2;
        y /= 2;

        // add margins
        x += 2;
        y += 2;

        targetSize = Mathf.Max(5, Mathf.Max(x / cam.aspect, y));
    }
}
