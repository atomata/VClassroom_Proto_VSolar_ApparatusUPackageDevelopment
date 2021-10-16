using UnityEngine;

namespace HexUN.Physics3D
{
    /// <summary>
    /// Good way to pause rigidbody
    /// Taken from: https://answers.unity.com/questions/284068/pauseing-and-resuming-a-rigidbody.html
    /// </summary>
    public class RigidbodyPauser
    {
        public bool IsRunning { get; private set; } = true;

        private Rigidbody _Body;
        private Vector3 _SavedVelocity;
        private Vector3 _SavedAngularVelocity;

        #region API
        public RigidbodyPauser(Rigidbody Body)
        {
            _Body = Body;
        }

        public void Pause()
        {
            _SavedVelocity = _Body.velocity;
            _SavedAngularVelocity = _Body.angularVelocity;
            _Body.isKinematic = true;
            IsRunning = false;
        }

        public void Resume()
        {
            _Body.isKinematic = false;
            _Body.AddForce(_SavedVelocity, ForceMode.VelocityChange);
            _Body.AddTorque(_SavedAngularVelocity, ForceMode.VelocityChange);
            IsRunning = true;
        }
        #endregion
    }
}