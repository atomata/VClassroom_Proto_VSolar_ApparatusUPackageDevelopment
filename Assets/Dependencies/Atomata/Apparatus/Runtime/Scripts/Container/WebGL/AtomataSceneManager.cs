using System.IO;
using Atomata.VSolar.Apparatus.Example;
using HexUN.Framework.Debugging;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    // TODO: Abstract an interface or abstract class for multiple platform
    public class AtomataSceneManager : MonoBehaviour
    {
        private const string cLogCategory = nameof(AtomataSceneManager);
        
        /// <summary>
        /// The url used to load assetbundles
        /// </summary>
        public string ContainerURL { get => _containerURL; set => _containerURL = value;}
        [SerializeField] private string _containerURL = "https://addressabletest1.blob.core.windows.net/assetbundles";

        public ApparatusContainer Container {get => _container;set => _container = value; }
        [SerializeField] ApparatusContainer _container;
        
        private IGameObjectProvider _assetProvider;
        private IApparatusProvider _apparatusProvider;
        private MaterialProvider _skyboxProvider;
        
        public string ContainerUrl;
        public Skybox skybox;
        public Camera Camera;
        
        void Start()
        {
            _skyboxProvider = new MaterialProvider(ContainerUrl + $"/skyboxes");
            _assetProvider = new CloudAssetProvider(ContainerUrl+ $"/assetbundles");
            _apparatusProvider = new CloudApparatusProvider(ContainerUrl+ $"/apparatus");

            Container.SceneManager = this;
        }
        
#if UNITY_EDITOR
        public string TestLoadKey;
        public string TestLoadSkyboxKey;
        public string TestTriggerKey;

        [ContextMenu("TestLoad")] void TestLoad() => LoadApparatus(TestLoadKey);
        [ContextMenu("TestUnload")] void TestUnload() => UnloadApparatus();
        [ContextMenu("TestLoadSkybox")] void TestLoadSkybox() => LoadSkybox(TestLoadSkyboxKey);
        [ContextMenu("TestTrigger")] void TestTrigger() => Trigger(TestTriggerKey);
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
        
        public void HandleRequest(ApparatusRequest request, LogWriter log)
        {
            log.AddInfo(cLogCategory, cLogCategory, $"Received request {request.RequestObject.Type}");
            UTApparatusRequest.HandleRequest(_assetProvider, _apparatusProvider, request, this, cLogCategory, log);
        }
        
        public void Trigger(string trigger)
        {
            _container.Trigger(ApparatusTrigger.FromPathString(trigger));
        }
    }
}
