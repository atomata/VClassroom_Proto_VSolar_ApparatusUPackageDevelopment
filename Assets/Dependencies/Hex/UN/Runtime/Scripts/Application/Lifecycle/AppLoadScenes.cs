using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.App
{
    [CreateAssetMenu(fileName = "AppLoadScenes", menuName = "HexUN/App/AppLoadScenes") ]
    public class AppLoadScenes : ScriptableObject
    {
        public string LoadingScreen;
        public float MinLoadTime = 2f;
        public string[] TemproaryScenes;
        public string[] PermanentScenes;
    }
}