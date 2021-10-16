using System.Collections;
using System.Collections.Generic;

using HexUN.Math;
using UnityEngine;
using HexUN.Framework;

namespace HexUN.Animation
{
    /// <summary>
    /// Singleton that runs all interpolations that occur in the game. This class
    /// simply provides the numbers requested over time and events to manage the interpolations
    /// occurence. Start, Cancel, Etc. 
    /// </summary>
    public class InterpolationManager : AOneHexPersistent<InterpolationManager>
    {
        // id is just an iteration on an int for now. 
        private int _id = int.MinValue;

        private Dictionary<int, Coroutine> _interpolationCoroutines = new Dictionary<int, Coroutine>();

        private Dictionary<int, InterpolationRequest> _interpolationCancelationTokens = new Dictionary<int, InterpolationRequest>();
        
        #region API
        /// <summary>
        /// Returns a unique id that can be retrieved and cached by an
        /// object that needs to use the interpolationManager
        /// </summary>
        /// <returns></returns>
        public int GetUniqueId()
        {
            int id = _id;
            _id++;
            return id;
        }

        /// <summary>
        /// Start an interpolation from start to end for the given duration. On inteprolation 
        /// retursn an array of size interpolations, providing an interpolation float for each.
        /// INTERPOLATION IS CURRENTLY EFFECTED BY TIME SCALE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public IInterpolationToken<float[]> StartInterpolation(int id, float duration, params SInterpolation[] interpolations)
        {
            if (_interpolationCancelationTokens.TryGetValue(id, out InterpolationRequest args))
            {
                args.Cancel();
                _interpolationCancelationTokens.Remove(id);
                _interpolationCoroutines.Remove(id);
            }

            InterpolationRequest newArgs = new InterpolationRequest(id, duration, interpolations);
            _interpolationCancelationTokens[id] = newArgs;
            _interpolationCoroutines[id] = StartCoroutine(Interpolate(newArgs));
            return newArgs;
        }
        #endregion

        IEnumerator Interpolate(InterpolationRequest args)
        {
            if (args.Interpolations.Length == 0) yield break;

            int interCount = args.Interpolations.Length;
            Interpolation[] interpolations = new Interpolation[interCount];

            for(int i = 0; i< interCount; i++)
            {
                interpolations[i] = new Interpolation(
                    args.Interpolations[i].Start,
                    args.Interpolations[i].End,
                    args.Duration,
                    args.Interpolations[i].Ease
                );
            }

            float duration = args.Duration;
            float time = 0;

            while (duration > time)
            {
                if (args.Cancelled == true)
                {
                    args.OnInterpolationEnd.Invoke();
                    yield break;
                }

                args.OnInterpolation.Invoke(MultiInterpolate());
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }

            time = duration;
            args.OnInterpolation.Invoke(MultiInterpolate());
            args.OnInterpolationEnd.Invoke();

            _interpolationCoroutines.Remove(args.Id);
            _interpolationCancelationTokens.Remove(args.Id);

            float[] MultiInterpolate()
            {
                float[] value = new float[interCount];

                for (int i = 0; i < interCount; i++)
                {
                    value[i] = interpolations[i].Interpolate(time);
                }
                return value;
            }
        }
    }
}