using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using Atomata.Scene;
using Atomata.VSolar.Apparatus.Example;
using Atomata.VSolar.Apparatus.UnityEditor;
using HexUN.Framework.Debugging;
using Unity.Serialization.Json;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class AtomataSceneManager : MonoBehaviour
    {
        private const string cLogCategory = nameof(AtomataSceneManager);

        private IGameObjectProvider _assetProvider;
        private IApparatusProvider _apparatusProvider;
        private MaterialProvider _skyboxProvider;

        private GameObject originCamera;
        private GameObject lerpStart;
        private GameObject lerpStop;
        
        [DllImport("__Internal")]
        private static extern void OnAtomataSceneInitalized();
        
        
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

        [SerializeField]
        [Tooltip("TEMP: If not null, makes the scene use the editable database in editor")]
        private SoApparatusConfig AssetDatabaseConfig;
        
        void Start()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            WebGLInput.captureAllKeyboardInput = false;
#endif
        
            if (_configSo != null)
                Config = _configSo.AsSceneConfig();
            
            if(Container != null)
                Container.SceneManager = this;
            else
                Debug.LogError($"No Container has been set");
            
            ConfigureSkybox();

            originCamera = new GameObject("originTransform");
            originCamera.transform.SetParent(_camera.transform, false);
            originCamera.transform.SetParent(null);

            lerpStart = new GameObject("lerpStart");
            lerpStop = new GameObject("lerpStop");

#if UNITY_WEBGL && !UNITY_EDITOR
            OnAtomataSceneInitalized();
#endif
        }

        void ConfigureProviders()
        {
            if (Config != null)
            {
                _skyboxProvider = new MaterialProvider(Config.SkyboxContainerUrl);

                #if UNITY_EDITOR
                if (AssetDatabaseConfig != null)
                {
                    _apparatusProvider = new EditableDatabaseApparatusProvider(AssetDatabaseConfig);
                    _assetProvider = new EditableDatabaseGameObjectProvider(AssetDatabaseConfig);
                }
                else
                {
                    _apparatusProvider = new CloudApparatusProvider(Config.ApparatusContainerUrl);
                    _assetProvider = new CloudAssetProvider(Config.AssetContainerUrl);
                } 
                #else
                    _apparatusProvider = new CloudApparatusProvider(Config.ApparatusContainerUrl);
                    _assetProvider = new CloudAssetProvider(Config.AssetContainerUrl);
                #endif
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

        public void ReturnCamera()
        { 
            StopAllCoroutines();

            lerpStart.transform.position = _camera.transform.position;
            lerpStart.transform.rotation = _camera.transform.rotation;

            lerpStop.transform.position = originCamera.transform.position;
            lerpStop.transform.rotation = originCamera.transform.rotation;
            
            StartCoroutine(LerpCamera(lerpStart.transform, lerpStop.transform, Config.CameraSpeed));
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

            if (!request.IsClaimed)
            {
                if (request.RequestObject.Type == EApparatusRequestType.CameraFocus && request.TryClaim(this))
                {
                    Transform target = request.RequestObject.Args as Transform;

                    lerpStart.transform.position = _camera.transform.position;
                    lerpStart.transform.rotation = _camera.transform.rotation;
                    
                    lerpStop.transform.position = target.position;
                    lerpStop.transform.rotation = target.rotation;

                    StartCoroutine(LerpCamera(lerpStart.transform, lerpStop.transform, Config.CameraSpeed));

                    request.Respond(null, this);
                }
                else
                {
                    UTApparatusRequest.HandleRequest(_assetProvider, _apparatusProvider, request, this, cLogCategory, log);
                }
            }
        }
        
        public void Trigger(string trigger)
        {
            _container.Trigger(ApparatusTrigger.FromPathString(trigger));
        }

        public void Dbg(string command)
        {
            switch (command.ToLower())
            {
                case "printtree":
                    Container.Debug_PrintTree();
                    break;
            }
        }
        
        IEnumerator LerpCamera(Transform origin, Transform target, float speed)
        {
            float lerp = 0;

            while (lerp < speed)
            {
                float t = lerp / speed;
                _camera.transform.position = Vector3.Lerp(origin.position, target.position, t);
                _camera.transform.rotation = Quaternion.Lerp(origin.rotation, target.rotation, t);
                lerp += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _camera.transform.position = target.position;
            _camera.transform.rotation = target.rotation;
        }
    }
}
