using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace HexUN.Engine
{
    /// <summary>
    /// Useful helper classes for transforms
    /// </summary>
    public static class UTTransform
    {
        /// <summary>
        /// Sets the pos, rot and scale of the transform in local space
        /// </summary>
        public static void Set(this Transform target, Vector3 pos, Quaternion rot, Vector3 scale)
        {
            target.position = pos;
            target.localRotation = rot;
            target.localScale = scale;
        }

        /// <summary>
        /// Copies the parent, position, localRotation and localScale from another transform
        /// </summary>
        public static void CopyFrom(this Transform target, Transform from)
        {
            if(target.parent != null) target.parent.SetParent(from.parent);
            target.position = from.position;
            target.localRotation = from.localRotation;
            target.localScale = from.localScale;
        }

        /// <summary>
        /// Recurively goes through all children and applies an action to all encountered children and 
        /// the oriignal transform
        /// </summary>
        public static void DoToSelfAndChildren(this Transform target, Action<Transform> action)
        {
            Queue<Transform> toProcess = new Queue<Transform>();
            toProcess.Enqueue(target);

            while(toProcess.Count > 0)
            {
                Transform process = toProcess.Dequeue();
                action(process);
                foreach(Transform child in process) toProcess.Enqueue(child);
            }
        }

        /// <summary>
        /// Get all the children of the transform as an array
        /// </summary>
        public static Transform[] GetAllChildren(this Transform target)
        {
            Transform[] children = new Transform[target.childCount];

            int i = 0;
            foreach (Transform child in target) children[i++] = child;
            return children;
        }

        /// <summary>
        /// Gets all the children of a transform as gameobjects
        /// </summary>
        public static GameObject[] GetAllChildrenAsGameObjects(this Transform target)
        {
            GameObject[] children = new GameObject[target.childCount];

            int i = 0;
            foreach (Transform child in target) children[i++] = child.gameObject;
            return children;
        }
    }
}