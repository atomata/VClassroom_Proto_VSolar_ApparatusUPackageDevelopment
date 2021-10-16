using HexUN.Math;
using HexUN.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.Animation
{
    public static class UTInterpolation
    {
        /// <summary>
        /// Sets up an inteprolation for a vector 3
        /// </summary>
        /// <param name="id"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static IInterpolationToken<Vector3> InterpolateVector3(int id, Vector3 from, Vector3 to, float duration, EEasingFunction ease)
        {
            SInterpolation[] interp = new SInterpolation[] 
            {
                new SInterpolation(from.x, to.x, ease),
                new SInterpolation(from.y, to.y, ease),
                new SInterpolation(from.z, to.z, ease)
            };

            IInterpolationToken<float[]> t = InterpolationManager.Instance.StartInterpolation(id, duration, interp);

            return Vector3InterpolationToken.FromInterpolationToken(t);
        }

        /// <summary>
        /// Interpolate a vector3 path
        /// </summary>
        /// <param name="id"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IInterpolationToken<Vector3> InterpolateVector3Path(int id, float duration, EEasingFunction ease, params Vector3[] path)
        {
            PathInterpolation<Vector3> pathInterp = new PathInterpolation<Vector3>(id, duration, ease, InterpolateVector3, path);
            pathInterp.StartPathInterpolation();
            return pathInterp;
        }
    }
}