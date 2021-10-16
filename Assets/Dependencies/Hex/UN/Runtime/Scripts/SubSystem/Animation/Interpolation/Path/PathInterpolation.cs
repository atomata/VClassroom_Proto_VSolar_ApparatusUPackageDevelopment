using HexCS.Core;
using HexUN.Math;
using System;
using System.Collections.Generic;

namespace HexUN.Animation
{
    /// <summary>
    /// Allows interpolating a sequence of T interpolation, and encasulates in a single InterpoationToken
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PathInterpolation<T> : IInterpolationToken<T>
    {
        private Event<T> _onInterpolation = new Event<T>();
        private Event _onInterpolationEnd = new Event();
        private Event _onInterpolationCanceled = new Event();

        private CreateToken _createTokenFunction;

        private int _id;
        private float _duration;
        private EEasingFunction _ease;
        private T[] _path;

        private int _currentStep = -1;

        private IInterpolationToken<T> _currentInterpolation;

        #region API
        public IEventSubscriber<T> OnInterpolationSubscriber => _onInterpolation;

        public IEventSubscriber OnInterpolationEndSubscriber => _onInterpolationEnd;

        public IEventSubscriber OnInterpolationCanceledSubscriber => _onInterpolationCanceled;

        public void Cancel() => _currentInterpolation.Cancel();

        /// <summary>
        /// Create a path interpolation along the given path
        /// </summary>
        /// <param name="id"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <param name="path"></param>
        public PathInterpolation(int id, float duration, EEasingFunction ease, CreateToken tokenFunc, params T[] path)
        {
            if (path.Length < 2) throw new ArgumentException("Interpolation paths cannot be less than 2 steps");

            _id = id;
            _duration = duration;
            _ease = ease;
            _path = path;
            _createTokenFunction = tokenFunc;
        }

        public void StartPathInterpolation()
        {
            PerformNextPathInterpolation();
        }
        #endregion
        
        private void PerformNextPathInterpolation()
        {
            _currentStep++;

            if (_currentStep > _path.Length - 2) return;
            if (_currentStep == _path.Length - 2) _onInterpolationEnd.Invoke();

            _currentInterpolation = _createTokenFunction(_id, _path[_currentStep], _path[_currentStep + 1], _duration, _ease);

            _currentInterpolation.OnInterpolationEndSubscriber.Subscribe(PerformNextPathInterpolation);
            _currentInterpolation.OnInterpolationCanceledSubscriber.Subscribe(_onInterpolationCanceled.Invoke);
            _currentInterpolation.OnInterpolationSubscriber.Subscribe(_onInterpolation.Invoke);
        }

        #region Internal Objects
        /// <summary>
        /// This function is the function used to create a substep in the path
        /// </summary>
        /// <param name="id"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public delegate IInterpolationToken<T> CreateToken(int id, T from, T to, float duration, EEasingFunction ease);
        #endregion
    }
}