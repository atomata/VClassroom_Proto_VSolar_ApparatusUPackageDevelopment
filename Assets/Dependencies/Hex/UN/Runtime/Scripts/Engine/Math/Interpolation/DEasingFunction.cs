using System;
using System.Collections.Generic;
using System.Text;

namespace HexUN.Math
{
    /// <summary>
    /// easing funciton take in some value between 0 and 1 and return
    /// another value between 0 and 1. The new value is the interpolation
    /// </summary>
    /// <param name="t">time between 0 and 1</param>
    /// <returns></returns>
    public delegate float DEasingFunction(float t);
}
