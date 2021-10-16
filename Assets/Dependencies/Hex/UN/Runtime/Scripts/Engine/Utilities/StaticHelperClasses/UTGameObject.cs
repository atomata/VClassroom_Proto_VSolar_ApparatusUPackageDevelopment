using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

namespace HexUN.Engine.Utilities
{
    public static class UTGameObject
    {
        /// <summary>
        /// Creates a game object that is completely empty, but has the same
        /// transform and parent as the original go
        /// </summary>
        public static GameObject CopyEmpty(this GameObject go)
        {
            GameObject cp = new GameObject($"{go.name}_EmptyCopy");
            cp.transform.CopyFrom(go.transform);
            return cp;
        }

        /// <summary>
        /// Destroy all children. Unless is a predicate that, if it returns true the child is not destoryed
        /// </summary>
        public static void DestroyAllChildren(this GameObject go, Predicate<GameObject> unless = null)
        {
            if(unless == null)
            {
                foreach (Transform child in UTTransform.GetAllChildren(go.transform)) UnityEngine.Object.Destroy(child.gameObject);
            }
            else
            {
                foreach (Transform child in UTTransform.GetAllChildren(go.transform))
                {
                    if(!unless(child.gameObject)) UnityEngine.Object.Destroy(child.gameObject);
                }
            }

        }

        /// <summary>
        /// DestroyImmedicate all children. Unless is a predicate that, if it returns true the child is not destoryed
        /// </summary>
        public static void DestroyAllChildrenImmediate(this GameObject go, Predicate<GameObject> unless = null)
        {
            if (unless == null)
            {
                foreach (Transform child in UTTransform.GetAllChildren(go.transform)) UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
            else
            {
                foreach (Transform child in UTTransform.GetAllChildren(go.transform))
                {
                    if (!unless(child.gameObject)) UnityEngine.Object.DestroyImmediate(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Destroy all children of a gameobject.
        /// regular destroy
        /// </summary>
        public static void DestroyAllChildren_EditorSafe(this GameObject go, Predicate<GameObject> unless = null)
        {
#if UNITY_EDITOR
            if (Application.isPlaying) go.DestroyAllChildren(unless);
            else go.DestroyAllChildrenImmediate(unless);
#else
            go.DestroyAllChildren();
#endif
        }

        public static void Destroy_EditorSafe(this GameObject go)
        {
#if UNITY_EDITOR
            if (Application.isPlaying) UnityEngine.Object.Destroy(go);
            else UnityEngine.Object.DestroyImmediate(go);
#else
            UnityEngine.Object.Destroy(go);
#endif
        }

        /// <summary>
        /// Creates a generic failure object
        /// </summary>
        public static GameObject CreateFailureObject()
        {
            GameObject ob = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ob.GetComponent<MeshRenderer>().material = null;
            return ob;
        }

        /// <summary>
        /// Instantiates a gameobject that is a child of this game object 
        /// </summary>
        public static GameObject AddChild(this GameObject target, string name)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(target.transform);
            return obj;
        }

        /// <summary>
        /// Instantiates a gameobject that is a child of this game object 
        /// </summary>
        public static T AddChild<T>(this GameObject target, string name)
            where T:MonoBehaviour
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(target.transform);
            return obj.AddComponent<T>();
        }

        /// <summary>
        /// Perform an action on every Gameobject in a GameObject Hierarchy. The action returns a bool.
        /// If it return false, then the current gameobjects children are not processed. If it returns 
        /// true, the children are also processed. Passing a gameobject will automtically skip the
        /// provided gameobject and begin processing it's children. 
        /// </summary>
        public static void DoToHierarchy(GameObject target, Predicate<GameObject> action)
        {
            DoToHierarchy(target.transform.GetAllChildrenAsGameObjects(), action);
        }

        /// <summary>
        /// Perform an action on every Gameobject in a GameObject Hierarchy. The action returns a bool.
        /// If it return false, then the current gameobjects children are not processed. If it returns 
        /// true, the children are also processed.
        /// </summary>
        public static void DoToHierarchy(GameObject[] target, Predicate<GameObject> action)
        {
            Queue<GameObject> hierarchy = new Queue<GameObject>(target);

            while(hierarchy.Count > 0)
            {
                GameObject process = hierarchy.Dequeue();

                if (action(process))
                {
                    foreach (Transform t in process.transform) hierarchy.Enqueue(t.gameObject);
                }
            }
        }

        /// <summary>
        /// Perform an action on every Gameobject in a GameObject Hierarchy. The action returns a bool.
        /// If it return false, then the current gameobjects children are not processed. If it returns 
        /// true, the children are also processed.
        /// </summary>
        public static async void DoToHierarchyAsync(GameObject[] target, Func<GameObject, Task<bool>> action)
        {
            Queue<GameObject> hierarchy = new Queue<GameObject>(target);

            while (hierarchy.Count > 0)
            {
                GameObject process = hierarchy.Dequeue();

                if (await action(process))
                {
                    foreach (Transform t in process.transform) hierarchy.Enqueue(t.gameObject);
                }
            }
        }
    }
}