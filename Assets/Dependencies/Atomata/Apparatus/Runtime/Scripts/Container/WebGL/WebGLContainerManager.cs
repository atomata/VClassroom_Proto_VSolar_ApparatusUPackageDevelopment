using System.IO;
using Atomata.VSolar.Apparatus.Example;
using HexUN.Framework.Debugging;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    // TODO: Abstract an interface or abstract class for multiple platform
    public class WebGLContainerManager : MonoBehaviour
    {
        public string ContainerUrl;
        private CloudSkyboxProvider _skyboxProvider;
        public Skybox skybox;
        
        [SerializeField] ApparatusContainer _container;
        public ApparatusContainer Container {get => _container;set => _container = value; }


        void Start()
        {
            _skyboxProvider = new CloudSkyboxProvider(ContainerUrl);
        }
        
#if UNITY_EDITOR
        public string TestLoadKey;
        public string TestLoadSkyboxKey;

        [ContextMenu("TestLoad")] void TestLoad() => LoadApparatus(TestLoadKey);
        [ContextMenu("TestUnload")] void TestUnload() => UnloadApparatus();
        [ContextMenu("TestLoadSkybox")] void TestLoadSkybox() => LoadSkybox(TestLoadSkyboxKey);
#endif

        public void LoadApparatus(string key)
        { 
            _container.LoadApparatus(key);
        }

        public void UnloadApparatus()
        {
            _container.UnloadApparatus();
        }

        public async void LoadSkybox(string key)
        {
            string prefix = Application.platform.AsAtomataPlatform().PlatformPrefix();
            Material m = await _skyboxProvider.Provide($"{key}_{prefix}", new LogWriter("temp"));
            skybox.material = m;
        }
        
        
        // Load Apparatus
        // Unload Apparatus
        // SendTrigger
    }
}
