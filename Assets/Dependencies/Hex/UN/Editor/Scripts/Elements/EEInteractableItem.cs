using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace HexUN.EditorElements
{
    /// <summary>
    /// Editor Element containing render functions for buttons
    /// </summary>
    public class EEInteractableItem : ALabeledEditorElement
    {
        /// <summary>
        /// Buttons
        /// </summary>
        public EEButton[] Buttons;

        /// <summary>
        /// Invoked when a button is clicked on the item. Returns the label of the clicked button
        /// </summary>
        public event Action<SLabel> OnInteraction;

        /// <summary>
        /// Construct an InteractableItem. Foreach buttonLabel, a button will be created
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <param name="buttonLabels"></param>
        public EEInteractableItem(SLabel itemLabel, params SLabel[] buttonLabels)
        {
            Label = itemLabel;

            if(buttonLabels != null)
            {
                Buttons = buttonLabels.Select(
                   l =>
                   {
                       EEButton b = new EEButton(l);
                       b.OnClick += () => OnInteraction?.Invoke(b.Label);
                       return b;
                   }
               ).ToArray();
            } else
            {
                Buttons = new EEButton[0];
            }
        }

        /// <summary>
        /// Construct an InteractableItem
        /// </summary>
        /// <param name="args"></param>
        public EEInteractableItem(Args args) : this(args.ItemLabel, args.ButtonLabels) { }

        /// <summary>
        /// Renders the element with mini buttons in a horizonal group
        /// </summary>
        public void Render_Horizontal(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(Content, options);

            foreach(EEButton b in Buttons)
            {
                b.Render_Mini();
            }

            EditorGUILayout.EndHorizontal();
        }

        private GUIContent CreateGuiContent() => new GUIContent(Label.ReadableName, Label.ToolTip);

        /// <summary>
        /// Arguments used to create an InteractableItem
        /// </summary>
        public struct Args
        {
            /// <summary>
            /// The label of the item
            /// </summary>
            public SLabel ItemLabel;

            /// <summary>
            /// Labels for the contained buttons
            /// </summary>
            public SLabel[] ButtonLabels;
        }
    }
}