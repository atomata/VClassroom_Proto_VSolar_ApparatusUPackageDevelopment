using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class CloudAssetProvider : ABlobContainerBasedProvider<GameObject>, IGameObjectProvider
    {
        public CloudAssetProvider(string containerURL) : base(containerURL + "/assetbundles")
        {
        }

        protected override GameObject Convert(byte[] bytes)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
            GameObject[] objs = assetBundle.LoadAllAssets<GameObject>();
            return objs != null && objs.Length > 0 ? objs[0] : null;
        }
    }
}