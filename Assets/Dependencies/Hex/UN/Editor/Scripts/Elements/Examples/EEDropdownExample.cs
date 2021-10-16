using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexUN.EditorElements
{
    /// <summary>
    /// Example InteractableList
    /// </summary>
    public class EEDropdownExample : EditorWindow
    {
        private EEDropdown _list;

        private string _response;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Hex/Examples/Elements/Dropdown")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(EEDropdownExample));
        }
        private void OnEnable()
        {
            List<EEDropdown.SElement> items = new List<EEDropdown.SElement>();

            for (int i = 0; i < 10; i++)
            {
                int rand = Random.Range(0, 10000);

                items.Add(
                    new EEDropdown.SElement()
                    {
                        ReadableName = $"Element {rand}",
                        Element = rand
                    }
                );
            }

            _list = new EEDropdown(items.ToArray());
        }

        private void OnGUI()
        {
            _list.Render_Basic();

            GUI.enabled = false;
            EditorGUILayout.TextArea($"{_list.Selected.ReadableName} is selected and has value {(int)_list.Selected.Element}");
            GUI.enabled = true;
        }
    }
}