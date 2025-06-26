using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SatValImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] Image pointer;
    RawImage satValImage;
    ColorPickerControl control;

    RectTransform rt, pointerRT;


    void Awake()
    {
        satValImage = GetComponent<RawImage>();
        control = GameObject.FindFirstObjectByType<ColorPickerControl>();
        rt = GetComponent<RectTransform>();
        pointerRT = pointer.GetComponent<RectTransform>();
        pointerRT.position = rt.sizeDelta * .5f;
    }


    void UpdateColor(PointerEventData eventData)
    {
        Vector3 position = rt.InverseTransformPoint(eventData.position);
        
        float deltaX = rt.sizeDelta.x * .5f;
        float deltaY = rt.sizeDelta.y * .5f;
        if (position.x < -deltaX)
            position.x = -deltaX;
        if (position.x > deltaX)
            position.x = deltaX;
        if (position.y < -deltaY)
            position.y = -deltaY;
        if (position.y > deltaY)
            position.y = deltaY;

        float x = position.x + deltaX;
        float y = position.y + deltaY;

        float normX = x / rt.sizeDelta.x;
        float normY = y / rt.sizeDelta.y;

        pointerRT.localPosition = position;

        control.UpdateSatVal(normX, normY);
    }


    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
}
