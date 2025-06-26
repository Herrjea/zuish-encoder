using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SatValImageControl : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] Image pointer;
    RawImage satValImage;
    ColorPickerControl control;
    Manager manager;

    RectTransform rt, pointerRT;

    [SerializeField] bool glyphColor = true;


    void Awake()
    {
        satValImage = GetComponent<RawImage>();
        control = GameObject.FindFirstObjectByType<ColorPickerControl>();
        manager = GameObject.FindFirstObjectByType<Manager>();
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
                rt.sizeDelta.x * (glyphColor ? control.currentSat : manager.currentSat) - rt.sizeDelta.x * .5f,
                rt.sizeDelta.y * (glyphColor ? control.currentVal : manager.currentSat) - rt.sizeDelta.y * .5f,
                0
            );
        }

        pointerRT.localPosition = position;

        // shift to all positives
        float x = position.x + deltaX;
        float y = position.y + deltaY;

        float normX = x / rt.sizeDelta.x;
        float normY = y / rt.sizeDelta.y;

        if (glyphColor)
            control.UpdateSatVal(normX, normY);
        else
            manager.UpdateSatVal(normX, normY);
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
