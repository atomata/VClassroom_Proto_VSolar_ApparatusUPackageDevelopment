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
    public class EESaveFileButtonExample : EditorWindow
    {
        EESaveFileButton _button;

        string _pathChosen;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Hex/Examples/Elements/SaveFileButton")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(EESaveFileButtonExample));
        }
        private void OnEnable()
        {
            Predicate<Type> test = UTType.Test_Or(
                UTType.ETypeTest.ABSTRACT,
                UTType.ETypeTest.ANONYMOUS,
                UTType.ETypeTest.GENERIC,
                UTType.ETypeTest.SEALED
            );

            _button = new EESaveFileButton(
                new SLabel() { Id = "save", ReadableName = "Save The File", ToolTip = "This saves the file" }
            );

            _button.OnPathChosen += s => _pathChosen = s;
        }

        private void OnGUI()
        {
            _button.Render_Basic("SAVE", "exe");

            GUI.enabled = false;
            EditorGUILayout.TextArea(_pathChosen);
            GUI.enabled = true;
        }
    }
}