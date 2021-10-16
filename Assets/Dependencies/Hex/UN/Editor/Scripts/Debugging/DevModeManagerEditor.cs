using UnityEditor;
using HexUN.Debugging;

namespace HexUN.Debugging
{
    [CustomEditor(typeof(DevModeManager))]
    [CanEditMultipleObjects]
    public class DevModeManagerEditor : ADevModeManagerEditor
    {
        protected override void HandleInspectorGUIAfterDevOptions() { }

        protected override void HandleOnEnable() { }
    }
}