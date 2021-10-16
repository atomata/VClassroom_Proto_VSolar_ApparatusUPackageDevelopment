using System;
using UnityEngine;
using UnityEditor;
using HexCS.Data.Persistence;
using HexUN.Data;
using HexCS.Core;

namespace HexUN.EditorElements
{
    /// <summary>
    /// Editor Element containing render functions for buttons
    /// </summary>
    public class EEOpenFileButton : ALabeledEditorElement
    {
        private UnityPath _lastSavePath = UnityPath.AssetsPath;

        /// <summary>
        /// Invoked when the user choses a path
        /// </summary>
        public event Action<UnityPath> OnPathChosen;

        /// <summary>
        /// Construct a button
        /// </summary>
        /// <param name="label"></param>
        public EEOpenFileButton(SLabel label) => Label = label;

        /// <summary>
        /// Renders the SaveFileButton
        /// </summary>
        /// <param name="defaultName">The default name of the file</param>
        /// <param name="extention">The extenstion of the file</param>
        /// <param name="options"></param>
        public void Render_Basic(string extention, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(Content, options))
            {
                UnityPath path = new PathString(EditorUtility.OpenFilePanel(Content.text, _lastSavePath, extention));

                if (path != null)
                {
                    OnPathChosen?.Invoke(path);
                }
            }
        }
    }
}