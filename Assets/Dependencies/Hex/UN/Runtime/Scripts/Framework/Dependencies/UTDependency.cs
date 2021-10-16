using UnityEngine;

namespace HexUN.Deps
{
    /// <summary>
    /// Contains useful functions for managing dependencies in Unity
    /// </summary>
    public static class UTDependency
    {
        /// <summary>
        /// Used in scripts as a shortcut to calling TryAsDep, and on failure
        /// setting the ob to null. This means in editor that dragging and object
        /// into a slot it cannot act as a dependency for will set it to null and log
        /// an error. Set canBeNull = true if you don't want errors on null inputs
        /// </summary>
        public static void Resolve<TExpected>(ref Object ob, out TExpected dep, MonoBehaviour dependentMono = null, bool canBeNull = false)
        {
            if (canBeNull && ob == null)
            {
                dep = default;
                return;
            }

            if (!ob.TryAsDep(out dep, dependentMono)) ob = null;
        }

        /// <summary>
        /// Try to cast the unity object to the dependency type. If fails, checks if object is a MonoBehaviour and tries to
        /// get a component of the dependency type. If this also fails, logs an error and returns default
        /// </summary>
        public static bool TryAsDep<TExpected>(this Object ob, out TExpected dep, MonoBehaviour dependentMono = null) 
            => TryAsDep((object)ob, out dep, dependentMono);

        private static bool TryAsDep<TExpected>(object ob, out TExpected dep, MonoBehaviour dependentMono)
        {
            // is the object the expected type
            if(ob is TExpected)
            {
                dep = (TExpected)ob;
                return true; 
            }

            // is the dependecy a monobehaviour component
            if(ob is GameObject)
            {
                GameObject obj = (GameObject)ob;

                TExpected comp = obj.GetComponent<TExpected>();

                if(comp != null)
                {
                    dep = comp;
                    return true;
                }
            }

            // Otherwise failed
            if (dependentMono == null)
            {
                Debug.LogError($"{ob} dependency extraction failed. {ob} is not type {typeof(TExpected).ToString()}");
            }
            else
            {
                Debug.LogError($"{ob} dependency extraction failed for mono {dependentMono} in gameobject {dependentMono.gameObject}. {ob} is not type {typeof(TExpected).ToString()}");
            }

            dep = default;
            return false;
        }
    }
}