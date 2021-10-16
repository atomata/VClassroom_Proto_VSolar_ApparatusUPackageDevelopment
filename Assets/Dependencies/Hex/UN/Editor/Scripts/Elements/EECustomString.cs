using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexUN.EditorElements
{
    /// <summary>
    /// A string whos values is either a custom value or a generated value
    /// </summary>
    public class EECustomString : ALabeledEditorElement
    {
        private Func<string> _generationFunction = null;

        private bool _isCustom;
        private string _customInput;

        public string String => _isCustom ? _customInput : _generationFunction();

        public EECustomString(SLabel label, Func<string> generationFunction)
        {
            Label = label;
            _generationFunction = generationFunction;
            _customInput = _generationFunction();
        }

        public void Render_Basic(params GUILayoutOption[] options)
        {
            _isCustom = EditorGUILayout.Toggle($"Is Custom {Label.ReadableName}", _isCustom);

            if (_isCustom)
            {
                _customInput = EditorGUILayout.TextField(Content, String);
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.TextField(Content, String);
                GUI.enabled = true;
            }
        }
    }
}