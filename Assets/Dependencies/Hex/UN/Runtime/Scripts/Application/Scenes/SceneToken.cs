using System;

namespace HexUN.App
{
    /// <summary>
    /// Represents a scene in the scene tracker
    /// </summary>
    [Serializable]
    public class SceneToken
    {
        public string Name;
        public string Tag;

        public SceneToken(string name, string tag = null)
        {
            Name = name;
            Tag = tag;
        }
    }
}
