using UnityEngine;

namespace HexUN.Engine.Utilities 
{ 
    /// <summary>
    /// Utilities pertaining to mono behaviour type objects
    /// </summary>
    public static class UTMonoBehaviour
    {
        /// <summary>
        /// Adds a child to the monobehaviours gameobject of type TChild 
        /// </summary>
        public static TChild AddChild<TChild>(this MonoBehaviour monoBehaviour, string name)
            where TChild: MonoBehaviour
        {
            return monoBehaviour.gameObject.AddChild<TChild>(name);
        }

        /// <summary>
        /// Adds a child to the monobehaviours gameobject of type TChild 
        /// </summary>
        public static void AddChild<TChild>(this MonoBehaviour monoBehaviour, string name, out TChild child)
            where TChild : MonoBehaviour
        {
            child = monoBehaviour.gameObject.AddChild<TChild>(name);
        }
    }
}
