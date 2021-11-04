using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public static class UTAtomataPlatform
    {
#if UNITY_EDITOR
        public static EAtomataPlatform EditorOnly_FromBuildTarget(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                    return EAtomataPlatform.Windows;
                case BuildTarget.WebGL:
                    return EAtomataPlatform.WebGL;
            }

            return EAtomataPlatform.Unknown;
        }
#endif

        public static EAtomataPlatform FromRuntimePlatform(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return EAtomataPlatform.Windows;
                case RuntimePlatform.WebGLPlayer:
                    return EAtomataPlatform.WebGL;
            }

            return EAtomataPlatform.Unknown;
        }
    }
}