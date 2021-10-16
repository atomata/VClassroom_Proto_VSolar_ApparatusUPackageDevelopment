using System;
using HexCS.Core;
using HexUN.Design;
using HexUN.EditorElements;
using UnityEditor;
using UnityEngine;

namespace HexUN.Events
{
    public class EWEventGenerator : EditorWindow
    {
        [SerializeField]
        private GameColorScheme _scheme;

        private static readonly GUIContent cWindowTitle = new GUIContent("EventGenerator");
        private static readonly GUIContent cSelectedTypeTitle = new GUIContent("Selected Type:");
        private static readonly GUIContent cEventNameTitle = new GUIContent("Event Name:");
        private static readonly GUIContent cEventListenerNameTitle = new GUIContent("Event Listener Name:");
        private static readonly GUIContent cUnityEventNameTitle = new GUIContent("Unity Event Name:");
        private static readonly GUIContent cMenuPathTitle = new GUIContent("Menu Path:");

        private EETypeList _selectedTypeList;
        private PathString _lastSavePath = null;

        private EECustomString _namespace;
        private EECustomString _menuPath;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Hex/Events/EventGenerator")]
        private static void CreateWindow()
        {
            // Get existing open window or if none, make a new one:
            EWEventGenerator window = (EWEventGenerator)GetWindow(typeof(EWEventGenerator));
            window.titleContent = cWindowTitle;
            window.Initialize();

            window.ShowPopup();
        }

        private void Initialize()
        {
            _selectedTypeList = new EETypeList(
                AppDomain.CurrentDomain.GetAssemblies(),
                1,
                UTType.Test_Or(
                    UTType.ETypeTest.CLASS,
                    UTType.ETypeTest.ENUM,
                    UTType.ETypeTest.INTERFACE,
                    UTType.ETypeTest.TOBIAS_VALUE_TYPE,
                    UTType.ETypeTest.STRUCT
                )
            );

            _namespace = new EECustomString(
                new SLabel() { ReadableName = "Namespace", ToolTip = "Namespace to use" },
                () => _selectedTypeList.SelectedType?.Namespace
            );

            _menuPath = new EECustomString(
                new SLabel() {
                    ReadableName = "Menu Name",
                    ToolTip = "Prefix in front of menu paths (Component and Asset). If this = MENU, then asset path is MENU/Events/EventName"
                },
                () => string.IsNullOrEmpty(_namespace?.String) ? 
                        "Unknown" 
                        : _namespace.String.Split('.')[0] + (_namespace.String.Split('.').Length < 2 ? "" : $"/{_namespace.String.Split('.')[1]}")
            );

            if (_lastSavePath == null)
            {
                _lastSavePath = new PathString(Application.dataPath);
            }
        }

        private void OnGUI()
        {
            if (_selectedTypeList == null) Initialize();

            _selectedTypeList.Render_Basic();

            EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
            _namespace.Render_Basic();
            _menuPath.Render_Basic();
            GUI.enabled = false;
            string type = EditorGUILayout.TextField(cSelectedTypeTitle, _selectedTypeList.SelectedType?.Name);
            EditorGUILayout.TextField(cEventNameTitle, $"{type?.EnforceFistCharCaptial()}Event");
            EditorGUILayout.TextField(cEventListenerNameTitle, $"{type?.EnforceFistCharCaptial()}EventListener");
            EditorGUILayout.TextField(cUnityEventNameTitle, $"{type?.EnforceFistCharCaptial()}UnityEventListener");
            EditorGUILayout.TextField(cMenuPathTitle, $"{_menuPath.String}/Events");
            GUI.enabled = true;

            if (GUILayout.Button("Save Event"))
            {
                PathString path = new PathString(EditorUtility.SaveFolderPanel("Generation Path", _lastSavePath, type));

                path = path.InsertAtEnd($"{type?.EnforceFistCharCaptial()}.event");
                path.CreateIfNotExistsDirectory();
                _lastSavePath = path.RemoveAtEnd();

                UTEventGeneration.GenerateEventsOfAllTypes(path, type, _namespace.String, $"{_menuPath.String}", _selectedTypeList.SelectedType.Namespace);
                AssetDatabase.Refresh();
            }
        }
    }
}