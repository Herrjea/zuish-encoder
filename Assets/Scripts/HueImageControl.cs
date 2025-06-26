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
    }


    void UpdateColor(PointerEventData data)
    {
        // get pointer position, clamped inside image
        // 0,0 is center
        Vector3 position;
        float deltaY = rt.sizeDelta.y * .5f;
        
        if (data != null)
        {
            position = rt.InverseTransformPoint(data.position);

            position.x = 0;
            if (position.y < -deltaY)
                position.y = -deltaY;
            if (position.y > deltaY)
                position.y = deltaY;
        }
        else
        {
            position = new Vector3(
                0,
                rt.sizeDelta.y * control.currentHue - rt.sizeDelta.y * .5f,
                0
            );
        }

        pointerRT.localPosition = position;

        // shift to all positives
        float y = position.y + deltaY;

        float normY = y / rt.sizeDelta.y;

        control.UpdateHue(normY);
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
