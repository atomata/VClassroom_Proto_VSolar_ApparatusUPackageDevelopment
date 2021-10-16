using System;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace HexUN.Utilities
{
    /// <summary>
    /// Utilities for lists of GameObjects
    /// </summary>
    public static class UTListGameObject
    {
        /// <summary>
        /// Forces the list to a new size either by reducing or growing. 
        /// Reducing destorys gameobjects with destroyFunction and removes from list, skipping null objects. 
        /// Growing instantiates them using instantiation function and adds to list.
        /// </summary>
        public static void ForceCount<TObject>(this List<TObject> target, int count, Func<TObject> instanationFunction, Action<TObject> destroyFunction)
            where TObject : Object
        {
            if (count == target.Count) return;

            if(count > target.Count)
            {
                while(target.Count < count)
                {
                    target.Add(instanationFunction());
                }
            } 
            else
            {
                while(target.Count > count)
                {
                    TObject go = target[target.Count - 1];
                    if (go != null) destroyFunction(go);
                    target.RemoveAt(target.Count - 1);
                }
            }
        }
    }
}