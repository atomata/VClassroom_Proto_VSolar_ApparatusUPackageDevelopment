using UnityEngine;

namespace HexUN.Edit
{
    /// <summary>
    /// Super general utilites to do with everything Unity Editor
    /// </summary>
    public static class UTEditor
    {
        private static Vector2 _toggleSizeCache;

        /// <summary>
        /// Gets the size of a basic regular toggle in unity
        /// </summary>
        public static Vector2 ToggleSize
        {
            get
            {
                if (_toggleSizeCache == default) _toggleSizeCache = GUI.skin.toggle.CalcSize(GUIContent.none);
                return _toggleSizeCache;
            }
        }
    }
}