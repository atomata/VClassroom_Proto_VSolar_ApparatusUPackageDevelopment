using HexUN.Behaviour;
using UnityEngine;
using HexCS.Core;
using Event = HexCS.Core.Event;
using HexUN.Framework.Services;
using HexUN.Framework;

namespace HexUN.Temporal
{
    /// <summary>
    /// Invokes an OnTickEvent on an interval.
    /// 
    /// Tracksw tick using ONUpdateCallback
    /// </summary>
    public class SimpleTimer
    {
        private bool _isDelayed;
        private float _remainingDelay;

        private float _currentTime = 0;
        private float _tickTarget;

        private Event _onTick = new Event();
        
        #region API
        /// <summary>
        /// Invoked on each tick
        /// </summary>
        public IEventSubscriber OnTick => _onTick; 

        /// <summary>
        /// Construct a simple timmer which calls on tick evenry
        /// tick seconds
        /// </summary>
        /// <param name="tick"></param>
        public SimpleTimer(float tick, float startDelay = 0)
        {
            _tickTarget = tick;

            _isDelayed = startDelay > 0;
            _remainingDelay = startDelay;

            OneHexServices.Instance.OnUpdate.Subscribe(HandleTick);
        }
        #endregion

        private void HandleTick()
        {
            float delta = Time.deltaTime;

            if (_isDelayed)
            {
                if(_remainingDelay > delta)
                {
                    _remainingDelay -= delta;
                }
                else
                {
                    _isDelayed = false;
                    float overflow = _remainingDelay - delta;
                    HandleCurrentTimeTick(overflow);
                }
            }

            // Handle the tick
            HandleCurrentTimeTick(delta);
        }

        private void HandleCurrentTimeTick(float delta)
        {
            _currentTime += delta;

            if (_currentTime >= _tickTarget)
            {
                _onTick?.Invoke();
                _currentTime -= _tickTarget;
            }
        }
    }
}