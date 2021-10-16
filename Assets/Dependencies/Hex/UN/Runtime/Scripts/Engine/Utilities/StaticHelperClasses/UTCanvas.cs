using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexUN.Utilities
{
    /// <summary>
    /// Utilites for Canvases in Unity
    /// </summary>
    public static class UTCanvas
    {
        /// <summary>
        /// Creates a world space canvas local to the game objects position
        /// with a 1, 1 size delta. Automatically addes a canvas scaler component
        /// </summary>
        /// <param name="parent">The paretn game object</param>
        /// <param name="localPosition">The local position of the resulting canvas</param>
        /// <param name="localRotation">The local rotation of the resulting canvas</param>
        /// <param name="unityUnitSize">The width height in unity units</param>
        /// <param name="canvasScale">The scale of the canvas, This can also be through of as pixels per unity unity per dimension. Keeps a uniform scale</param>
        /// <returns></returns>
        public static Canvas CreateLocalCanvas(GameObject parent, Vector3 localPosition, Quaternion localRotation, Vector2 unityUnitSize, float canvasScale)
        {
            Canvas c = parent.AddComponent<Canvas>();
            c.renderMode = RenderMode.WorldSpace;
            RectTransform rt = (RectTransform)c.transform;
            rt.localPosition = localPosition;
            rt.localRotation = localRotation;
            rt.localScale = Vector3.one * canvasScale;
            rt.sizeDelta = unityUnitSize;

            parent.AddComponent<CanvasScaler>();
            return c;
        }

        /// <summary>
        /// Adds text to a canvas centered with the font size and object name provided. 
        /// Used mostly for simple, debugging style text you want to lazy load
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="objectName"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public static TextMeshProUGUI AddTextCentered(Canvas parent, string objectName, float fontSize)
        {
            GameObject text = new GameObject(objectName, typeof(RectTransform));

            text.transform.SetParent(parent.transform, false);
            text.AddComponent<CanvasRenderer>();
            TextMeshProUGUI t = text.AddComponent<TextMeshProUGUI>();
            RectTransform textRect = (RectTransform)t.transform;
            t.fontSize = fontSize; 
            textRect.localScale = Vector3.one;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            t.alignment = TextAlignmentOptions.Center;
            return t;
        }
    }
}