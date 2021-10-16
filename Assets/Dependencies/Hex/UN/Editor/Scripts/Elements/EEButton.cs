using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexUN.EditorElements
{
    /// <summary>
    /// Editor Element containing render functions for buttons
    /// </summary>
    public class EEButton : ALabeledEditorElement
    {
        /// <summary>
        /// Invoked when the button is clicked
        /// </summary>
        public event Action OnClick;

        /// <summary>
        /// Construct a button
        /// </summary>
        /// <param name="label"></param>
        public EEButton(SLabel label) => Label = label;

        /// <summary>
        /// Renders the button
        /// </summary>
        public void Render_Basic(params GUILayoutOption[] options)
        {
            if (GUILayout.Button(Content, options))
            {
                OnClick?.Invoke();
            }
        }

        /// <summary>
        /// Renders the button with mini button style
        /// </summary>
        public void Render_Mini(params GUILayoutOption[] options)
        {
            if (GUILayout.Button(Content, EditorStyles.miniButton, options))
            {
                OnClick?.Invoke();
            }
        }
    }
}