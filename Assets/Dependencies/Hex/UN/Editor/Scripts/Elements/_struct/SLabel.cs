using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.EditorElements
{
    /// <summary>
    /// Contains Readable Name and id. Used for named UI elements that also need
    /// to be identified
    /// </summary>
    public struct SLabel
    {
        /// <summary>
        /// A Readable name for the element used by UI
        /// </summary>
        public string ReadableName;

        /// <summary>
        /// A Tooltip for the label
        /// </summary>
        public string ToolTip;

        /// <summary>
        /// The id of the element, used for comparisions. This will not be rendered
        /// in UIs
        /// </summary>
        public string Id;
    }
}