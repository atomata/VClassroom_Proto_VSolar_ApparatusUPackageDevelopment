using System.IO;
using Atomata.Scene;
using Atomata.VSolar.Apparatus.Example;
using HexUN.Framework.Debugging;
using Unity.Serialization.Json;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    // TODO: Abstract an interface or abstract class for multiple platform
    public class AtomataSceneManager : MonoBehaviour
    {
        private const string cLogCategory = nameof(AtomataSceneManager);

        private IGameObjectProvider _assetProvider;
        private IApparatusProvider _apparatusProvider;
        private MaterialProvider _skyboxProvider;
        
        public AtomataSceneConfig Config
        {
            get => _config;
            set
            {
                if (_config != value)
                {
                    _config = value;
                    ConfigureProviders();
                }
            }
        }
        private AtomataSceneConfig _config;
        [SerializeField] private AtomataSceneConfigSo _configSo;
        
        public Skybox Skybox
        {
            get => _skybox;
            set
            {
                if (_skybox != value)
                {
                    _skybox = value;
                    ConfigureSkybox();
                }
            }
        }
        [SerializeField] private Skybox _skybox;

        public Material SkyboxMaterial
        {
            get => _skyboxMaterial;
            set
            {
                if (_skyboxMaterial != value)
                {
                    _skyboxMaterial = value;
                    ConfigureSkybox();
                }
            }
        }
        [SerializeField] private Material _skyboxMaterial = null;
        
        public Camera Camera
        {
            get => _camera;
            set => _camera = value;
        }
        [SerializeField] private Camera _camera;
        
        public ApparatusContainer Container {get => _container;set => _container = value; }
        [SerializeField] ApparatusContainer _container;
        
        void Start()
        {
            if (_configSo != null)
                Config = _configSo.AsSceneConfig();
            
            if(Container != null)
                Container.SceneManager = this;
            else
                Debug.LogError($"No Container has been set");
            
            ConfigureSkybox();
        }

        void ConfigureProviders()
        {
            if (Config != null)
            {
                _skyboxProvider = new MaterialProvider(Config.SkyboxContainerUrl);
                _assetProvider = new CloudAssetProvider(Config.AssetContainerUrl);
                _apparatusProvider = new CloudApparatusProvider(Config.ApparatusContainerUrl);
            }
            else
                Debug.LogError($"No {nameof(Config)} has been set");
        }

        void ConfigureSkybox()
        {
            if (Skybox != null)
                Skybox.material = _skyboxMaterial;
            else
                Debug.LogError($"No Skybox component has been set");
        }

        public void LoadConfiguration(string configuration)
        {
            Debug.Log($"Receieved: {configuration}");
            AtomataSceneConfig config = JsonSerialization.FromJson<AtomataSceneConfig>(configuration);
            Config = config;
        }
        
        public void LoadApparatus(string key) => _container.LoadApparatus(key);

        public void UnloadApparatus() => _container.UnloadApparatus();

        public async void LoadSkybox(string key)
        {
            string prefix = Application.platform.AsAtomataPlatform().PlatformPrefix();
            SkyboxMaterial = await _skyboxProvider.Provide($"{key}_{prefix}", new LogWriter("temp"));
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
