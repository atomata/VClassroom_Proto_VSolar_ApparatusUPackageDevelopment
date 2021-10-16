using System;
using System.Collections.Generic;
using System.Linq;

namespace HexUN.Work
{
    /// <summary>
    /// A work token is used to track the work status of listeners, when a pattern requires that
    /// some event emitter does not have direct access to it's listeners. When the event emitter
    /// emits an event, a work token can be provides which allows listeners to register as dependent
    /// workers. This allows the emitter to check if workers have completed the required work bassed on 
    /// an emission. In some cases, further event emissions should be blocked until all listeners 
    /// work is complete. 
    /// </summary>
    public class WorkToken
    {
        private Dictionary<int, Func<EWorkState>> _registrationsCache;

        private int _lastId = int.MinValue;

        private Dictionary<int, Func<EWorkState>> _registrations
        {
            get
            {
                if (_registrationsCache == null) _registrationsCache = new Dictionary<int, Func<EWorkState>>();
                return _registrationsCache;
            }
        }

        /// <summary>
        /// Register as a worker and provide a callback that be be used to determine work state
        /// </summary>
        /// <param name="workStateCallback"></param>
        /// <returns></returns>
        public int RegisterAsWorked(Func<EWorkState> workStateCallback)
        {
            int id = _lastId++;
            _registrations.Add(id, workStateCallback);
            return id;
        } 
        
        /// <summary>
        /// Checks all registered workers. If all are Idle, return Idle. If 1 is Erroring, return Error. 
        /// If any number is Busy and all others are Idle, returns busy
        /// </summary>
        /// <returns></returns>
        public EWorkState GetWorkState()
        {
            if (_registrations.Count == 0) return EWorkState.Idle;

            IEnumerable<EWorkState> states = _registrations.Values.Where(s => s != null).Select(s => s());

            if (states.Contains(EWorkState.Error)) return EWorkState.Error;
            if (states.Contains(EWorkState.Busy)) return EWorkState.Busy;

            return EWorkState.Idle;
        }

        /// <summary>
        /// Get all callbacks (for custom queries)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Func<EWorkState>> GetCallbacks() => _registrations.Values;

        /// <summary>
        /// Clears the registration cache, useful for pooling. Once cleared, can reuse the work token
        /// </summary>
        public void Clear() => _registrationsCache?.Clear();
    }
}