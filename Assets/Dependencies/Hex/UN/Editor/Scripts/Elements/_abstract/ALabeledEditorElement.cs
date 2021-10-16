using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.EditorElements
{
    /// <summary>
    /// An editor element with a label that is rendered as a GUIContent
    /// </summary>
    public class ALabeledEditorElement
    {
        private SLabel _label;
        private GUIContent _content;

        /// <summary>
        /// GUIContent representing label
        /// </summary>
        protected GUIContent Content
        {
            get
            {
                if(
                    _content == null 
                    || _content.text != _label.ReadableName
                    || _content.tooltip != _label.ToolTip
                )
                {
                    _content = new GUIContent(_label.ReadableName, _label.ToolTip);
                }

                return _content;
            }
        }

        /// <summary>
        /// Element Label
        /// </summary>
        public SLabel Label
        {
            get => _label;
            set
            {
                _label = value;
            }
        }
    }
}