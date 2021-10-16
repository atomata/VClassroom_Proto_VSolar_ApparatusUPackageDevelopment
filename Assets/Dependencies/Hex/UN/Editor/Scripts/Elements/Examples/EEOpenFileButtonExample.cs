using System;
using System.Reflection;
using HexCS.Core;
using HexUN.App;
using UnityEditor;
using UnityEngine;

namespace HexUN.EditorElements
{ 
    /// <summary>
    /// Example InteractableList
    /// </summary>
    public class EEOpenFileButtonExample : EditorWindow
    {
        EEOpenFileButton _button;

        string _pathChosen;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Hex/Examples/Elements/OpenFileButton")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(EEOpenFileButtonExample));
        }
        private void OnEnable()
        {
            Predicate<Type> test = UTType.Test_Or(
                UTType.ETypeTest.ABSTRACT,
                UTType.ETypeTest.ANONYMOUS,
                UTType.ETypeTest.GENERIC,
                UTType.ETypeTest.SEALED
            );

            _button = new EEOpenFileButton(
                new SLabel() { Id = "open", ReadableName = "Open The File", ToolTip = "This opens the file" }
            );

            _button.OnPathChosen += s => _pathChosen = s;
        }

        private void OnGUI()
        {
            _button.Render_Basic("json");

            GUI.enabled = false;
            EditorGUILayout.TextArea(_pathChosen);
            GUI.enabled = true;
        }
    }
}