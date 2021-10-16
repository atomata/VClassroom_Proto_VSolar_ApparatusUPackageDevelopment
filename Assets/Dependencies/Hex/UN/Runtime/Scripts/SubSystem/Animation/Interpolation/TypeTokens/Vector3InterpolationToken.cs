using HexCS.Core;
using UnityEngine;
using Event = HexCS.Core.Event;

namespace HexUN.Animation
{
    public class Vector3InterpolationToken : IInterpolationToken<Vector3>
    {
        private Event _onInterpolationEnd = new Event();
        private Event _onInterpolationCanceled = new Event();
        private Event<Vector3> _onInterpolation= new Event<Vector3>();

        private IInterpolationToken<float[]> tok;

        #region API
        /// <inheritdoc />
        public IEventSubscriber OnInterpolationEndSubscriber => _onInterpolationEnd;

        /// <inheritdoc />
        public IEventSubscriber OnInterpolationCanceledSubscriber => _onInterpolationCanceled;

        /// <inheritdoc />
        public IEventSubscriber<Vector3> OnInterpolationSubscriber => _onInterpolation;

        public void Cancel() => tok?.Cancel();

        public static Vector3InterpolationToken FromInterpolationToken(IInterpolationToken<float[]> t)
        {
            Vector3InterpolationToken inst = new Vector3InterpolationToken();

            t.OnInterpolationCanceledSubscriber.Subscribe(inst._onInterpolationCanceled.Invoke);
            t.OnInterpolationEndSubscriber.Subscribe(inst._onInterpolationEnd.Invoke);
            t.OnInterpolationSubscriber.Subscribe(inst.HandleInterpolation);

            inst.tok = t;

            return inst;
        }

        #endregion
        private void HandleInterpolation(float[] interp)
        {
            _onInterpolation.Invoke(new Vector3(interp[0], interp[1], interp[2]));
        }

        private Vector3InterpolationToken() { }
    }
}