using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SatValImageControl : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] Image pointer;
    RawImage satValImage;
    [SerializeField] ColorPickerControl control;

    RectTransform rt, pointerRT;


    void Awake()
    {
        satValImage = GetComponent<RawImage>();
        rt = GetComponent<RectTransform>();
        pointerRT = pointer.GetComponent<RectTransform>();
        pointerRT.position = rt.sizeDelta * .5f;

        UpdateColor(null);
    }


    void UpdateColor(PointerEventData data)
    {
        // get pointer position, clamped inside image
        // 0,0 is center
        Vector3 position;
        float deltaX = rt.sizeDelta.x * .5f;
        float deltaY = rt.sizeDelta.y * .5f;
        if (data != null)
        {
            position = rt.InverseTransformPoint(data.position);

            if (position.x < -deltaX)
                position.x = -deltaX;
            if (position.x > deltaX)
                position.x = deltaX;
            if (position.y < -deltaY)
                position.y = -deltaY;
            if (position.y > deltaY)
                position.y = deltaY;
        }
        else
        {
            position = new Vector3(
                rt.sizeDelta.x * control.currentSat - rt.sizeDelta.x * .5f,
                rt.sizeDelta.y * control.currentVal - rt.sizeDelta.y * .5f,
                0
            );
        }

        pointerRT.localPosition = position;

        // shift to all positives
        float x = position.x + deltaX;
        float y = position.y + deltaY;

        float normX = x / rt.sizeDelta.x;
        float normY = y / rt.sizeDelta.y;

        control.UpdateSatVal(normX, normY);
    }


    public void OnDrag(PointerEventData data)
    {
        UpdateColor(data);
    }

    public void OnPointerDown(PointerEventData data)
    {
        UpdateColor(data);
    }
}
