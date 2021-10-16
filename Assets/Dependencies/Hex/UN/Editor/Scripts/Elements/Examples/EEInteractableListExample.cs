using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexUN.EditorElements
{
    /// <summary>
    /// Example InteractableList
    /// </summary>
    public class EEInteractableListExample : EditorWindow
    {
        private EEInteractableList _list;

        private string _response;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Hex/Examples/Elements/InteractableList")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(EEInteractableListExample));
        }
        private void OnEnable()
        {
            List<EEInteractableItem.Args> items = new List<EEInteractableItem.Args>();

            for (int i = 0; i < 100; i++)
            {
                int rand = Random.Range(0, 10000);

                items.Add(new EEInteractableItem.Args()
                {
                    ItemLabel = new SLabel() { Id = $"{rand}", ReadableName = $"CoolItem{rand}", ToolTip = $"Amazing tooltip {rand}" },
                    ButtonLabels = new SLabel[]
                    {
                    new SLabel() { Id = "on", ReadableName = $"On", ToolTip = $"Button ooon" },
                    new SLabel() { Id = "off", ReadableName = $"Off", ToolTip = $"Button oooof" },
                    }
                });
            }

            _list = new EEInteractableList(
                new SLabel() { Id = "List", ReadableName = "List Example", ToolTip = "This is a list example" },
                items.ToArray()
            );

            _list.OnItemInteraction += _list_OnItemInteraction;
        }

        private void OnGUI()
        {
            _list.Render_Basic(300);

            GUI.enabled = false;
            EditorGUILayout.TextArea(_response);
            GUI.enabled = true;
        }

        private void _list_OnItemInteraction(EEInteractableItem arg1, SLabel arg2)
        {
            _response = $"The item {arg1.Label.ReadableName} was clicked with the {arg2.ReadableName} action";
        }
    }
}