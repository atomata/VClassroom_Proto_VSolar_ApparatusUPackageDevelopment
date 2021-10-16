using UnityEditor;
using UnityEngine;

namespace HexUN.EditorElements
{ 
    /// <summary>
    /// Example InteractableList
    /// </summary>
    public class EECustomStringExample : EditorWindow
    {
        private EECustomString _string;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Hex/Examples/Elements/CustomString")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(EECustomStringExample));
        }
        private void OnEnable()
        {
            _string = new EECustomString(
                new SLabel() { Id = "id", ReadableName = "StringEx", ToolTip = "Example" },
                () => "Example"
            );
        }

        private void OnGUI()
        {
            _string.Render_Basic();

            GUI.enabled = false;
            EditorGUILayout.TextField(_string.String);
            GUI.enabled = true;
        }
    }
}