using UnityEngine;

namespace HexUN.Utilities
{
    /// <summary>
    /// Static helper for dealing with Unity Rect transforms. Mostly contains helper functions for
    /// adjusting the positions, pivots, etc. of RectTransforms
    /// </summary>
    public static class UTRectTransform
    {
        /// <summary>
        /// Stretches the rect transform so that it takes on the size of the
        /// parent. Modifies the provided transform
        /// </summary>
        /// <param name="trans"></param>
        public static void RectStretch(this RectTransform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
            trans.anchorMin = Vector2.zero;
            trans.anchorMax = Vector2.one;
            trans.anchoredPosition = Vector2.zero;
            trans.sizeDelta = Vector2.zero;
            trans.pivot = new Vector2(0.5f, 0.5f);
        }
    }
}