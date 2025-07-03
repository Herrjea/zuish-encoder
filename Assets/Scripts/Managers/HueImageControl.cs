using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HueImageControl : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] Image pointer;
    RawImage hueImage;
    [SerializeField] ColorPickerControl control;

    RectTransform rt, pointerRT;


    void Awake()
    {
        hueImage = GetComponent<RawImage>();
        rt = GetComponent<RectTransform>();
        pointerRT = pointer.GetComponent<RectTransform>();
        pointerRT.position = rt.sizeDelta * .5f;

        UpdateColor(null);

        GameEvents.RandomizeColors.AddListener(RandomizeColors);
    }


    void UpdateColor(PointerEventData data)
    {
        // get pointer position, clamped inside image
        // 0,0 is center
        Vector3 position;
        float deltaX = rt.sizeDelta.x * .5f;
        
        if (data != null)
        {
            position = rt.InverseTransformPoint(data.position);

            position.y = 0;
            if (position.x < -deltaX)
                position.x = -deltaX;
            if (position.x > deltaX)
                position.x = deltaX;
        }
        else
        {
            position = new Vector3(
                rt.sizeDelta.x * control.currentHue - rt.sizeDelta.x * .5f,
                0,
                0
            );
        }

        pointerRT.localPosition = position;

        // shift to all positives
        float x = position.x + deltaX;

        float normX = x / rt.sizeDelta.x;

        control.UpdateHue(normX);
    }


    public void OnDrag(PointerEventData data)
    {
        UpdateColor(data);
    }

    public void OnPointerDown(PointerEventData data)
    {
        UpdateColor(data);
    }

    void RandomizeColors()
    {
        UpdateColor(null);
    }
}
