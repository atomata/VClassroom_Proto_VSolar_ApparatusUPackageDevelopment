using UnityEngine;

namespace HexUN.Debugging
{
    public static class UTDevModeManagment
    {
        /// <summary>
        /// Handles hiding and showing scripts and gameobjects in a hierarchy that
        /// should be hidden when not developing the asset. This function hids all children of
        /// the given transform and the sibling mono behaviours (which should be on the same gamobject
        /// as the transform)
        /// </summary>
        /// <param name="isDevMode"></param>
        /// <param name="parent"></param>
        /// <param name="siblings"></param>
        public static void SetDevMode(bool isDevMode, Transform parent, params MonoBehaviour[] siblings)
        {
            HideFlags flag = isDevMode ?
                HideFlags.None :
                HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            foreach (Transform trans in parent)
            {
                if (trans == parent) continue;
                trans.hideFlags = flag;
            }

            if (siblings != null)
            {
                foreach (MonoBehaviour mb in siblings)
                {
                    if (mb != null) mb.hideFlags = flag;
                }
            }
        }
    }
}