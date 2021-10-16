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
    public class EEInteractableList : ALabeledEditorElement
    {
        private Vector2 _scrollPosition;

        /// <summary>
        /// Buttons
        /// </summary>
        public EEInteractableItem[] Items;

        /// <summary>
        /// Invoked when a button is clicked on an item. Returns the item and the SLabel of the interaction
        /// </summary>
        public event Action<EEInteractableItem, SLabel> OnItemInteraction;

        /// <summary>
        /// Construct an InteractableItem. Foreach buttonLabel, a button will be created
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <param name="buttonLabels"></param>
        public EEInteractableList(SLabel listLabel, params EEInteractableItem.Args[] interableItemArgs)
        {
            Label = listLabel;

            Items = interableItemArgs.Select(
                a =>
                {
                    EEInteractableItem item = new EEInteractableItem(a);
                    item.OnInteraction += l => OnItemInteraction?.Invoke(item, l);
                    return item;
                }
            ).ToArray();
        }

        /// <summary>
        /// Renders in vertical layout group with bold label and a scroll box
        /// </summary>
        /// <param name="dimensions">dimensions of box</param>
        public void Render_Basic(float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(Content, EditorStyles.boldLabel);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(height));

            foreach(EEInteractableItem i in Items)
            {
                i.Render_Horizontal();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}