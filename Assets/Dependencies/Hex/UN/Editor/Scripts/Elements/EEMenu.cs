using System.Collections.Generic;
using HexCS.Core;
using HexCS.Data.Persistence;
using UnityEditor;
using UnityEngine;

namespace HexUN.EditorElements
{
    /// <summary>
    /// An interactable list where a type can be selected
    /// </summary>
    public class EEMenu<TItem> : ALabeledEditorElement
    {
        // UNFINISHED, NEEDS COMPLETING


        private SMenuItem[] _content;

        private GenericMenu _menu;
        private TItem _selected;

        private Queue<SMenuItem> _newContent = new Queue<SMenuItem>();

        public EEMenu(params SMenuItem[] elements)
        {
            _menu = new GenericMenu();
            _content = elements;
        }

        public void Render_Basic()
        {
            while(_newContent.Count > 0)
            {
                SMenuItem newCont = _newContent.Dequeue();
                _menu.AddItem(new GUIContent(newCont.Path), false, HandleItemClicked, newCont.Item);
            }

            _menu.ShowAsContext();
        }

        private void HandleItemClicked(object selected)
        {
            _selected = (TItem)selected;
        }

        /// <summary>
        /// An element that can be used in a dropdown menu
        /// </summary>
        public struct SMenuItem
        {
            public PathString Path;
            public TItem Item; 
        }
    }
}