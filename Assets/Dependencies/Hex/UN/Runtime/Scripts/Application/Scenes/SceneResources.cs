using HexUN.Behaviour;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HexUN.App
{
    /// <summary>
    /// Holds the resources required by the scene and provides options on
    /// when they should be enabled or disabled
    /// </summary>
    public class SceneResources : HexBehaviour
    {
        private const string cEditorResourceTag = "EditorOnly";

        [Header("Options")]
        [SerializeField]
        private GameObject[] _resources = null;

        private bool _isActive = false;
        private GameObject[] _instances = null;

        // Used to track the scene resources that are loaded. This uses the name of the SceneResource gameobject
        private static List<string> _loadedSceneResources;

        protected override void HexAwake()
        {
  
            if (_loadedSceneResources == null || !_loadedSceneResources.Contains(name))
            {
                InstantiatePlayModeResources();
                _isActive = true;
            }
                
            

            //Destroy(gameObject);
        }

        /// <inheritdoc />
        protected void OnValidate()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChange;
            EditorApplication.playModeStateChanged += HandlePlayModeStateChange;
#endif

            if (!_isActive && gameObject.activeInHierarchy)
            {
                if (!Application.isPlaying)
                {
                    InstantiateEditorResources();
                    _isActive = true;
                }
            }
        }

        private void InstantiateEditorResources()
        {
            if (_resources == null) return;
            ClearInstanceCache();

            _instances = new GameObject[_resources.Length];

#if UNITY_EDITOR
            for(int i = 0; i<_resources.Length; i++)
            {
                GameObject inst = (GameObject)PrefabUtility.InstantiatePrefab(_resources[i], transform);
                inst.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
                _instances[i] = inst;
            }
#endif
        }

        private void InstantiatePlayModeResources()
        {
            if (_resources == null) return;
            ClearInstanceCache();

#if UNITY_EDITOR
            _instances = new GameObject[_resources.Length];
            Scene sc = SceneManager.CreateScene(name);

            for (int i = 0; i < _resources.Length; i++)
            {
                GameObject inst = (GameObject)PrefabUtility.InstantiatePrefab(_resources[i]);
                inst.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
                _instances[i] = inst;

                SceneManager.MoveGameObjectToScene(inst, sc);
            }
#endif

            if (_loadedSceneResources == null) _loadedSceneResources = new List<string>();
            _loadedSceneResources.Add(name);
        }

        private void ClearInstanceCache()
        {
            if (_instances != null)
            {
                foreach (GameObject instance in _instances)
                {
                    DestroyImmediate(instance);
                }
            }
        }

#if UNITY_EDITOR
        private void HandlePlayModeStateChange(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingEditMode)
            {
                ClearInstanceCache();
            }
        }
#endif
    }
}