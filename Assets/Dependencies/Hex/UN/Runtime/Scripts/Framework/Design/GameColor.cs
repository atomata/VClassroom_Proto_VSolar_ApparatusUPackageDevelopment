using System;
using UnityEngine;

namespace HexUN.Design
{
    /// <summary>
    /// Provides game color infomation, which groups a color
    /// to a few useful alternatives that can be used by many objects
    /// </summary>
    [CreateAssetMenu(fileName = "GameColor", menuName = "HexUN/UI/GameColor")]
    [Serializable]
    public class GameColor : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Base color")]
        private Color _base = default;

        [SerializeField]
        [Tooltip("Light color")]
        private Color _light = default;

        [SerializeField]
        [Tooltip("Dark color")]
        private Color _dark = default;

        [SerializeField]
        [Tooltip("Greyed out color (for UI)")]
        private Color _greyed = default;

        [SerializeField]
        [Tooltip("Base color")]
        private Material _baseMaterial = null;

        [SerializeField]
        [Tooltip("Light color")]
        private Material _lightBaseMaterial = null;

        [SerializeField]
        [Tooltip("Dark color")]
        private Material _darkBaseMaterial = null;

        [SerializeField]
        [Tooltip("Greyed out color (for UI)")]
        private Material _greyedBaseMaterial = null;

        public Color Base => _base;
        public Color Light => _light;
        public Color Dark => _dark;
        public Color Greyed => _greyed;
        public Material BaseMaterial => _baseMaterial;
        public Material LightMaterial => _lightBaseMaterial;
        public Material DarkMaterial => _darkBaseMaterial;
        public Material GreyedMaterial => _greyedBaseMaterial;
    }
}