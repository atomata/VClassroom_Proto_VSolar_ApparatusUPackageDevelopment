using HexUN.Framework;

using UnityEngine;

namespace HexUN.App
{
    /// <summary>
    /// Exposes important application control unsing an interface
    /// </summary>
    public class OneAppControl : AOneHexPersistent<OneAppControl>, IAppControl
    {
        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}