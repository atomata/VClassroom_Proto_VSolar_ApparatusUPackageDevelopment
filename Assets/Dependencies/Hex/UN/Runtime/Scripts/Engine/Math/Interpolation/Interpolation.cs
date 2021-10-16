using System;
using System.Collections.Generic;
using System.Text;

namespace HexUN.Math
{
    /// <summary>
    /// Interpolation is the act of using an easing function to find a value between y0 and y1
    /// based on a time value (t) witin a duration. It is used commonly in animation. 
    /// </summary>
    public class Interpolation
    {
        private DEasingFunction _ease;

        /// <summary>
        /// Y Max. when t = diration, Y Max is returned
        /// </summary>
        public float Y1 { get; private set; }

        /// <summary>
        /// Y Min. when t = 0, Y Min is returned
        /// </summary>
        public float Y0 { get; private set; }

        /// <summary>
        /// Distance between Y0 and Y1
        /// </summary>
        public float YSpan { get; private set; }

        /// <summary>
        /// The duration which t is tested against. Must be > 0
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// The Easing function used by the interpolation
        /// </summary>
        public EEasingFunction EasingFunction { get; private set; }

        /// <summary>
        /// Construct an interpolation
        /// </summary>
        /// <param name="y0">min value on y axis</param>
        /// <param name="y1">max value on y axis</param>
        /// <param name="duration">duration of interpolation</param>
        /// <param name="ease">easing function</param>
        public Interpolation(float y0, float y1, float duration = 1, EEasingFunction ease = EEasingFunction.Linear)
        {
            if (duration == 0) throw new ArgumentException("Duration cannot = 0 in an interpolation");

            Y1 = y1;
            Y0 = y0;
            Duration = duration;
            EasingFunction = ease;
            _ease = ease.Function();
            YSpan = Y1 - Y0;
        }

        /// <summary>
        /// Takes in a time value and interpolated between y1 and y0
        /// based on the duration. Uses the easing function provided at construct time
        /// </summary>
        /// <param name="t">time, normally between 0 and duration, technically can be out of bounds</param>
        /// <returns></returns>
        public float Interpolate(float t) => _ease(t / Duration) * YSpan + Y0;

        /// <summary>
        /// Takes in a time value and interpolated between y1 and y0
        /// based on the duration. Uses the easing functin provided as an argument
        /// </summary>
        /// <param name="t">time, normally between 0 and duration, technically can be out of bounds</param>
        /// <param name="ease">The easing funciton to use</param>
        /// <returns></returns>
        public float Interpolate(float t, EEasingFunction ease) => ease.Function()((t / Duration) * YSpan + Y0);
    }
}
