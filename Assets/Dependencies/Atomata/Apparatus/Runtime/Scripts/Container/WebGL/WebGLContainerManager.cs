using System.IO;
using Atomata.VSolar.Apparatus.Example;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    // TODO: Abstract an interface or abstract class for multiple platform
    public class WebGLContainerManager : MonoBehaviour
    {
        [SerializeField] ApparatusContainer _container;
        public ApparatusContainer Container {get => _container;set => _container = value; }

#if UNITY_EDITOR
        public string TestLoadKey;

        [ContextMenu("TestLoad")] void TestLoad() => LoadApparatus(TestLoadKey);
        [ContextMenu("TestUnload")] void TestUnload() => UnloadApparatus();
#endif

        public void LoadApparatus(string key)
        { 
            _container.LoadApparatus(key);
        }

        public void UnloadApparatus()
        {
            _container.UnloadApparatus();
        }
        
        
        // Load Apparatus
        // Unload Apparatus
        // SendTrigger
    }
}
