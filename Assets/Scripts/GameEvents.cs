using UnityEngine;
using UnityEngine.Events;

public class GameEvents
{
    public class V3Event : UnityEvent<Vector3> {}
    public class FloatFloatEvent : UnityEvent<float, float> {}

    public static V3Event NewTextCenter = new V3Event();
    public static FloatFloatEvent NewTextSize = new FloatFloatEvent();
}
