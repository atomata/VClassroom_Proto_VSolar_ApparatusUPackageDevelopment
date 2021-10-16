using System;
using System.Collections.Generic;
using System.Text;

namespace HexUN.Math
{
    /// <summary>
    /// Easing functions commonly using in interpolations. 
    /// Implementations taken from: http://gizma.com/easing/.
    /// My functions work like this. 0 to 1 always.
    /// 1 |        x
    ///   |      x
    ///   |    x
    ///   |  x
    /// 0 |x _ _ _ _
    ///    0       1
    /// </summary>
    public enum EEasingFunction
    {
        /// <summary>
        /// Simple linear function
        /// </summary>
        Linear = 0,

        /// <summary>
        /// Simple Ease
        /// </summary>
        In_Quadratic = 1,

        /// <summary>
        /// Simple ease out
        /// </summary>
        Out_Quadratic = 2,

        /// <summary>
        /// Ease in and out
        /// </summary>
        InOut_Quadratic = 3,

        /// <summary>
        /// Cubic ease in
        /// </summary>
        In_Cubic = 4,

        /// <summary>
        /// cubic ease out
        /// </summary>
        Out_Cubic = 5,

        /// <summary>
        /// Cubic ease in out
        /// </summary>
        InOut_Cubic = 6,

        /// <summary>
        /// Quartic ease in
        /// </summary>
        In_Quartic = 7,

        /// <summary>
        /// Quartic ease out
        /// </summary>
        Out_Quartic = 8,

        /// <summary>
        /// Quartic ease in out
        /// </summary>
        InOut_Quartic = 9,

        /// <summary>
        /// Quintic ease in
        /// </summary>
        In_Quintic = 10,

        /// <summary>
        /// Quintic ease out
        /// </summary>
        Out_Quintic = 11,

        /// <summary>
        /// Quintic ease in out
        /// </summary>
        InOut_Quintic = 12,

        /// <summary>
        /// Sinusoidal ease in
        /// </summary>
        In_Sinusoidal = 13,

        /// <summary>
        /// Sinusoidal ease out
        /// </summary>
        Out_Sinusoidal = 14,

        /// <summary>
        /// Sinusoidal ease in out
        /// </summary>
        InOut_Sinusoidal = 15,

        /// <summary>
        /// Exponential ease in
        /// </summary>
        In_Exponential = 16,

        /// <summary>
        /// Exponential ease out
        /// </summary>
        Out_Exponential = 17,

        /// <summary>
        /// Exponential ease in out
        /// </summary>
        InOut_Exponential = 18,

        /// <summary>
        /// Circular ease in
        /// </summary>
        In_Circular = 19,

        /// <summary>
        /// Circular ease out
        /// </summary>
        Out_Circular = 20,

        /// <summary>
        /// Circular ease in out
        /// </summary>
        InOut_Circular = 21
    }

    /// <summary>
    /// Utility for easing functions
    /// </summary>
    public static class UTEEasingFunction
    {
        private const float cPI = (float)System.Math.PI;
        private const float cPIHalf = (float)(System.Math.PI / 2);


        /// <summary>
        /// Returns the appropriate function for the Easing function. Functions are only guarenteed
        /// to wokr withing a range of (0, 1). Most funcitno will work outside these bounds, but
        /// some will throw math errors
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static DEasingFunction Function(this EEasingFunction func)
        {
            switch (func)
            {
                case EEasingFunction.Linear: return Linear;
                case EEasingFunction.In_Quadratic: return QuadraticEaseIn;
                case EEasingFunction.Out_Quadratic: return QuadraticEaseOut;
                case EEasingFunction.InOut_Quadratic: return QuadraticEaseInOut;
                case EEasingFunction.In_Cubic: return CubicEaseIn;
                case EEasingFunction.Out_Cubic: return CubicEaseOut;
                case EEasingFunction.InOut_Cubic: return CubicEaseInOut;
                case EEasingFunction.In_Quartic: return QuarticEaseIn;
                case EEasingFunction.Out_Quartic: return QuarticEaseOut;
                case EEasingFunction.InOut_Quartic: return QuarticEaseInOut;
                case EEasingFunction.In_Quintic: return QuinticEaseIn;
                case EEasingFunction.Out_Quintic: return QuinticEaseOut;
                case EEasingFunction.InOut_Quintic: return QuinticEaseInOut;
                case EEasingFunction.In_Sinusoidal: return SinusoidalEaseIn;
                case EEasingFunction.Out_Sinusoidal: return SinusoidalEaseOut;
                case EEasingFunction.InOut_Sinusoidal: return SinusoidalEaseInOut;
                case EEasingFunction.In_Exponential: return ExponentialEaseIn;
                case EEasingFunction.Out_Exponential: return ExponentialEaseOut;
                case EEasingFunction.InOut_Exponential: return ExponentialEaseInOut;
                case EEasingFunction.In_Circular: return CircularEaseIn;
                case EEasingFunction.Out_Circular: return CircularEaseOut;
                case EEasingFunction.InOut_Circular: return CircularEaseInOut;
            }

            return null;
        }

        private static float Linear(float t) => t;
        private static float QuadraticEaseIn(float t) => t * t;
        private static float QuadraticEaseOut(float t) => -(t * (t - 2));
        private static float QuadraticEaseInOut(float t)
        {
            float t1 = t / 0.5f;
            if (t1 < 1) return 0.5f * t1 * t1;
            t1--;
            return -0.5f * (t1 * (t1 - 2) - 1);
        }
        private static float CubicEaseIn(float t) => t * t * t;
        private static float CubicEaseOut(float t)
        {
            t--;
            return (t * t * t + 1);
        }
        private static float CubicEaseInOut(float t)
        {
            float t1 = t / 0.5f;
            if (t1 < 1) return 0.5f * (t1 * t1 * t1);
            t1 -= 2;
            return 0.5f * (t1 * t1 * t1 + 2);
        }

        private static float QuarticEaseIn(float t) => t * t * t * t;
        private static float QuarticEaseOut(float t)
        {
            t--;
            return -(t * t * t * t - 1);
        }
        private static float QuarticEaseInOut(float t)
        {
            t /= 0.5f;
            if (t < 1) return 0.5f * (t * t * t * t);
            t -= 2;
            return -0.5f * (t * t * t * t - 2);
        }

        private static float QuinticEaseIn(float t) => t * t * t * t * t;
        private static float QuinticEaseOut(float t)
        {
            t--;
            return -(t * t * t * t - 1);
        }
        private static float QuinticEaseInOut(float t)
        {
            t /= 0.5f;
            if (t < 1) return 0.5f * (t * t * t * t * t);
            t -= 2;
            return 0.5f * (t * t * t * t * t + 2);
        }

        private static float SinusoidalEaseIn(float t) => (float)(-(System.Math.Cos(t * cPIHalf)) + 1);
        private static float SinusoidalEaseOut(float t) => (float)System.Math.Sin(t * cPIHalf);
        private static float SinusoidalEaseInOut(float t) => (float)(-0.5 * (System.Math.Cos(cPI * t) - 1));

        private static float ExponentialEaseIn(float t) => t == 0 ? 0 : (float)(System.Math.Pow(2, 10 * (t - 1)));
        private static float ExponentialEaseOut(float t) => t == 1 ? 1 : (float)-System.Math.Pow(2, -10 * t) + 1;
        private static float ExponentialEaseInOut(float t)
        {
            if (t == 0) return 0;
            if (t == 1) return 1;

            t /= 0.5f;
            if (t < 1) return (float)(0.5 * System.Math.Pow(2, 10 * (2 * t - 1)));
            t--;
            return (float)(0.5 * ((-System.Math.Pow(-2, -10 * t)) + 2));
        }

        private static float CircularEaseIn(float t) => (float)-(System.Math.Sqrt(1 - t * t) - 1);
        private static float CircularEaseOut(float t)
        {
            t--;
            return (float)(System.Math.Sqrt(1 - t * t));
        }

        private static float CircularEaseInOut(float t)
        {
            t /= 0.5f;
            if (t < 1) return (float)(-0.5 * (System.Math.Sqrt(1 - t * t)-1));
            t -= 2;
            return (float)(0.5*(System.Math.Sqrt(1-t*t)+1));
        }
    }
}
