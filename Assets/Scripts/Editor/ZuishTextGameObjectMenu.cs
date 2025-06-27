using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class ZuishTextGameObjectMenu
{
    [MenuItem("GameObject/UI/Zuish text", false, 10)]
    static void CreateCustomObject(MenuCommand menuCommand)
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        GameObject zuishText = new GameObject("Zuish text");

        ZuishText text = zuishText.AddComponent<ZuishText>();
        text.Init();

        RectTransform rt = zuishText.AddComponent<RectTransform>();
        rt.SetParent(canvas.transform, false);
        rt.sizeDelta = new Vector2(100, 100);

        // Allow nesting under the selected UI element
        if (menuCommand.context is GameObject contextObj && contextObj.GetComponentInParent<Canvas>() != null)
        {
            zuishText.transform.SetParent(contextObj.transform, false);
        }

        // Ensure it gets parented to the currently selected object (if any)
        //GameObjectUtility.SetParentAndAlign(zuishText, menuCommand.context as GameObject);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(zuishText, "Create " + zuishText.name);

        Selection.activeObject = zuishText;
    }
}
