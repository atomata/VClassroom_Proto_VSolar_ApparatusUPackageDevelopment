using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;

namespace HexUN.Engine.Utilities
{
    public class UTPrefab
    {
#if UNITY_EDITOR
        /// <summary>
        /// Applyes instance modifications to a prefab parent if there
        /// are any. Otherwise returns false;
        /// </summary>
        public static bool TryApplyOverrides(GameObject instance)
        {
            List<ObjectOverride> modications = PrefabUtility.GetObjectOverrides(instance);

            if (modications.Count == 0) return false;

            foreach(ObjectOverride mod in modications)
            {
                string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(mod.GetAssetObject());
                mod.Apply(path, InteractionMode.AutomatedAction);
            }

            return true;
        }
#endif
    }
}