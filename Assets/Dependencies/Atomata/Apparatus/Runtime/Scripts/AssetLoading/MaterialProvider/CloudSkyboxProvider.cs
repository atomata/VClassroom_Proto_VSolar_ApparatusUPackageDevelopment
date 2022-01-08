using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class CloudSkyboxProvider : ABlobContainerBasedProvider<Material>
    {
        public CloudSkyboxProvider(string containerURL) : base(containerURL + "/skyboxes")
        {
        }

        protected override Material Convert(byte[] bytes)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
            Material[] objs = assetBundle.LoadAllAssets<Material>();
            return objs != null && objs.Length > 0 ? objs[0] : null;
        }
    }
}