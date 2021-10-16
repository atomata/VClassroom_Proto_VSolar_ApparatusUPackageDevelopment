using System;
using UnityEngine;

namespace HexUN.Design
{
    /// <summary>
    /// Provides game color infomation, which groups a color
    /// to a few useful alternatives that can be used by many objects
    /// </summary>
    [CreateAssetMenu(fileName = "GameColorScheme", menuName = "HexUN/UI/GameColorScheme")]
    [Serializable]
    public class GameColorScheme : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Primary Color")]
        private GameColor _primary = null;

        [SerializeField]
        [Tooltip("Secondary Color")]
        private GameColor _secondary = null;

        [SerializeField]
        [Tooltip("Alternative Color")]
        private GameColor _alternative = null;

        [SerializeField]
        [Tooltip("Background Color")]
        private GameColor _background = null;

        [SerializeField]
        [Tooltip("Neutral colors for netural elemtents like text")]
        private GameColor _neutral = null;

        public GameColor Primary => _primary;
        public GameColor Secondary => _secondary;
        public GameColor Alternative => _alternative;
        public GameColor Background => _background;
        public GameColor Neutral => _neutral;
    }
}