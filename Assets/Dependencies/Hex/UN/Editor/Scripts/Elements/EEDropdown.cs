using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HexUN.EditorElements
{
    /// <summary>
    /// An interactable list where a type can be selected
    /// </summary>
    public class EEDropdown : ALabeledEditorElement
    {
        /// <summary>
        /// Elements in the dropdown
        /// </summary>
        public SElement[] Elements;

        /// <summary>
        /// The selected element
        /// </summary>
        public SElement Selected => Elements[_index];

        private GUIContent[] _content;
        private int _index;

        public EEDropdown(params SElement[] elements)
        {
            Elements = elements;

            _content = Elements.Select(e => new GUIContent(e.ReadableName)).ToArray();
            _index = 0;
        }

        public void Render_Basic()
        {
            _index = EditorGUILayout.Popup(_index, _content);
        }

        /// <summary>
        /// An element that can be used in a dropdown menu
        /// </summary>
        public struct SElement
        {
            public string ReadableName;
            public object Element;
        }
    }
}