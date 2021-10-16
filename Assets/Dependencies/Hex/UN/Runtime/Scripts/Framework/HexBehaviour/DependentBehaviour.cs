using HexCS.Core;

using HexUN.Framework.Behaviour;

using UnityEngine;

namespace HexUN.Behaviour
{
    /// <summary>
    /// Adds useful functionality for managing interface dependencies of objects.
    /// - Provides an overridable <see cref="ResolveDependencies"></see> function to resolve iterface dependencies in gameobjects
    /// - Provides an overridable <see cref="ResolveEventBindings"></see> to bind events to resolved dependencies
    /// - Automatically handles the validation functionality of dependencies to enforce the dependencies have correct interface
    /// </summary>
    public abstract class DependentBehaviour : SceneBehaviour
    {
        #region Protected API
        /// <inheritdoc>
        protected override void HexAwake()
        {
            ResolveDependencies();
            EventBindings.ClearAndUnsubscribeAll();
            ResolveEventBindings(EventBindings);
#if HEXDB
            LogInfo<MonoDependent>($"Awake complete. Dependencies and EventBindings resolved");
#endif
        }

        /// <inheritdoc>
        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            // This is commented out because this is happening right as you click play in editor.
            // i.e. It's being validated in some editor phase where the object is set to null (probably during initialization)

            //if (!Application.isPlaying)
            //{
            //    ResolveDependencies();
            //    EventBindings.ClearAndUnsubscribeAll();
            //    ResolveEventBindings(EventBindings);
            //}
#endif
        }

        /// <summary>
        /// Use UTDependency to resolve serializedObject type variable to
        /// interface containers in the prefab
        /// </summary>
        protected virtual void ResolveDependencies() { }

        /// <summary>
        /// bind events that need to be bound from dependencies. This will occur after
        /// dependencies are resolved
        /// </summary>
        /// <param name="ebs"></param>
        protected virtual void ResolveEventBindings(EventBindingGroup ebs) { }
        #endregion
    }
}