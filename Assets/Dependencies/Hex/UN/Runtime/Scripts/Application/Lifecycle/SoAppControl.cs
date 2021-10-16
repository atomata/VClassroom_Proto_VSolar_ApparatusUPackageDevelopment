using UnityEngine;

namespace HexUN.App
{
    /// <summary>
    /// Exposes important application control unsing an interface
    /// </summary>
    [CreateAssetMenu(fileName = "AppControl", menuName = "HexUN/Services/AppControl")]
    public class SoAppControl : ScriptableObject, IAppControl
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
