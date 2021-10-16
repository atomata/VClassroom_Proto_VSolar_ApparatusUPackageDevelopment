using HexCS.Core;

using HexUN.Framework.Services;
using HexUN.Behaviour;

using UnityEngine;

namespace HexUN.Framework.SharedResource
{
    /// <summary>
    /// This is a resource that is shared between multiple classes. Classes can
    /// listen for changes.
    /// </summary>
    public abstract class ASOSharedResource : ScriptableObject, ISharedResource
    {
        /// <summary>
        /// Invoked an uupdate is pushed to the resource
        /// </summary>
        private HexCS.Core.Event _onUpdated = new HexCS.Core.Event();

        private bool _isUpdatePushed = false;

        private OneHexServices instRef = null;

        #region Protected API
        /// <summary>
        /// Clear the data. This is used because ScriptableObjects serialize
        /// and save their data in editor
        /// </summary>
        public abstract void Clear();
        #endregion

        #region API
        /// <inheritdoc/>
        public IEventSubscriber OnUpdated => _onUpdated;

        /// <inheritdoc/>
        public void PushUpdate()
        {
            if (instRef == null)
            {
                instRef = OneHexServices.Instance;
                instRef.OnLateUpdate.Subscribe(DoPushUpdate);
            }

            _isUpdatePushed = true;
        }
        #endregion

        private void DoPushUpdate()
        {
            if (_isUpdatePushed)
            {
                _onUpdated.Invoke();
                _isUpdatePushed = false;
            }
        }
    }
}