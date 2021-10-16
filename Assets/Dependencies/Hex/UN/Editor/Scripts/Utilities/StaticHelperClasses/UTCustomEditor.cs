using UnityEditor;
using UnityEngine;

namespace HexUN.Edit
{
    public static class UTCustomEditor
    {
        /// <summary>
        /// Show disabled reference to a mono behaviour object in custom editor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ShowMonoReference(MonoBehaviour mb)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mb), typeof(MonoBehaviour), false);
            GUI.enabled = true;
        }

        /// <summary>
        /// Show disabled reference to a scriptable object object in custom editor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ShowSOReference(ScriptableObject so)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(so), typeof(ScriptableObject), false);
            GUI.enabled = true;
        }

        /// <summary>
        /// Show disabled reference to a scriptable object object in custom editor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Header(string label)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }
    }
}