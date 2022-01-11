using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using Unity.Serialization;

namespace Atomata.Scene
{
    [CreateAssetMenu(fileName = nameof(AtomataSceneConfigSo), menuName = "Atomata/AtomataSceneConfig")]
    public class AtomataSceneConfigSo : ScriptableObject
    {
        public string ApparatusContainerUrl;
        public string AssetContainerUrl;
        public string SkyboxContainerUrl;
        public float CameraSpeed = 1.5f;

        public AtomataSceneConfig AsSceneConfig()
        {
            return new AtomataSceneConfig()
            {
                ApparatusContainerUrl = ApparatusContainerUrl,
                AssetContainerUrl = AssetContainerUrl,
                SkyboxContainerUrl = SkyboxContainerUrl,
                CameraSpeed = CameraSpeed
            };
        }
    }

    [GeneratePropertyBag]
    public class AtomataSceneConfig
    {
        public string ApparatusContainerUrl;
        public string AssetContainerUrl;
        public string SkyboxContainerUrl;
        public float CameraSpeed;
    }
}