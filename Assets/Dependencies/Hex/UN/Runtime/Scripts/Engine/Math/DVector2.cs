using System;
using System.Collections;
using System.Collections.Generic;
using HexCS.Core;
using UnityEngine;

namespace HexUN.Math
{
    /// <summary>
    /// Serializable version of a DVector2
    /// </summary>
    [Serializable]
    public struct DVector2
    {
        /// <summary>
        /// The X value of the coordinate
        /// </summary>
        public int X;

        /// <summary>
        /// The Y value of the coordinate
        /// </summary>
        public int Y;

        /// <summary>
        /// Make DVector2 with two int coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public DVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values summed
        /// </summary>
        /// <param name="vec1">First GridCoordinate2</param>
        /// <param name="vec2">Second GridCoordinate2</param>
        /// <returns>addition</returns>
        public static DVector2 operator +(DVector2 vec1, DVector2 vec2)
        {
            return new DVector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values subtracted.
        /// </summary>
        /// <param name="vec1">Minuhend</param>
        /// <param name="vec2">Subtrahend</param>
        /// <returns>difference</returns>
        public static DVector2 operator -(DVector2 vec1, DVector2 vec2)
        {
            return new DVector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        /// <inheritdoc />
        public static bool operator ==(DVector2 vec1, DVector2 vec2) => vec1.X == vec2.X && vec1.Y == vec2.Y;

        /// <inheritdoc />
        public static bool operator !=(DVector2 vec1, DVector2 vec2) => !(vec1.X == vec2.X && vec1.Y == vec2.Y);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DVector2 vector && X == vector.X && Y == vector.Y;

        /// <inheritdoc />
        public override int GetHashCode() => UTHash.BasicHash(X, Y);

        public static implicit operator DiscreteVector2(DVector2 v) => new DiscreteVector2(v.X, v.Y);

        public static implicit operator DVector2(DiscreteVector2 v) => new DVector2(v.X, v.Y);
    }
}