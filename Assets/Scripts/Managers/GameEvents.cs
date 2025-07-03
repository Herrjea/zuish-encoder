using UnityEngine;
using UnityEngine.Events;

public class GameEvents
{
    public class V3Event : UnityEvent<Vector3> {}
    public class FloatFloatEvent : UnityEvent<float, float> {}
    public class ColorEvent : UnityEvent<Color> {}

    public static V3Event NewTextCenter = new V3Event();
    public static FloatFloatEvent NewTextSize = new FloatFloatEvent();

    public static ColorEvent NewGlyphColor = new ColorEvent();
    public static ColorEvent NewBGColor = new ColorEvent();

    public static UnityEvent RandomizeColors = new UnityEvent();
}
