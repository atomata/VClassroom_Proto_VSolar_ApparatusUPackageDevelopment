using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexUN.Debugging
{



    public static class UTDebug
    {
        [MenuItem("Hex/Utility/Cleanup Missing Scripts")]
        static void CleanupMissingScripts()
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(Selection.gameObjects[i]);
            }
        }
    }
}