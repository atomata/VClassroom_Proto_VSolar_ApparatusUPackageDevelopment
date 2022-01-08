using System;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public enum EAtomataPlatform
    {
        Unknown,
        Windows,
        WebGL
    }

    public static class UTEAtomataPlatform
    {
        public static EAtomataPlatform AsAtomataPlatform(this RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return EAtomataPlatform.Windows;
                case RuntimePlatform.WebGLPlayer:
                    return EAtomataPlatform.WebGL;
                default:
                    return EAtomataPlatform.Unknown;
            }
        }

        public static string PlatformPrefix(this EAtomataPlatform platform)
        {
            return Enum.GetName(typeof(EAtomataPlatform), platform)?.ToLower();
        }
    }
}