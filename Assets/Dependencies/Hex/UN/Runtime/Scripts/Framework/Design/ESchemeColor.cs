using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.Design
{
    /// <summary>
    /// The color to pull from a color scheme
    /// </summary>
    public enum ESchemeColor
    {
        /// <summary>
        /// Primary color in scheme. Should be most prevalent
        /// </summary>
        Primary,

        /// <summary>
        /// Secondary scheme color is used in conguction with primary
        /// when more variation is required (example, toggle)
        /// </summary>
        Secondary,

        /// <summary>
        /// Alternative color can be similar or different to others. 
        /// Used in rare occasions and for highlights
        /// </summary>
        Alternative,

        /// <summary>
        /// Color to use as the background
        /// </summary>
        Background,

        /// <summary>
        /// Neutral color. Used often for text
        /// </summary>
        Neutral
    }

    public static class UTESchemeColor
    {
        /// <summary>
        /// Returns the gaem color corresponding to the procided color enum. Returns null if invalid color enum provided.
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static GameColor GetGameColor(this GameColorScheme scheme, ESchemeColor color)
        {
            switch (color)
            {
                case ESchemeColor.Alternative: return scheme.Alternative;
                case ESchemeColor.Background: return scheme.Background;
                case ESchemeColor.Primary: return scheme.Primary;
                case ESchemeColor.Secondary: return scheme.Secondary;
                case ESchemeColor.Neutral: return scheme.Neutral;
            }

            return null;
        }
    }
}