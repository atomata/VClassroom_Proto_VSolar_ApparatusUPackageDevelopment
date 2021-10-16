using HexUN.Behaviour;

namespace HexUN.App
{
    /// <summary>
    /// Base class that handles tracking of the quitting event on a MonBehviour.
    /// Used in classes the require knowledge of MonoBehaviours quit status. Good
    /// example is Singletons. 
    /// </summary>
    public abstract class AQuitter : HexBehaviour
    {
        /// <summary>
        /// Is the Unity application quitting
        /// </summary>
        public static bool AppQuitting { get; private set; }
        
        private void OnApplicationQuit() => AppQuitting = true;
    }
}