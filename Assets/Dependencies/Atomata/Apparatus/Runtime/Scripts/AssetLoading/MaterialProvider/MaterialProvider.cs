using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class MaterialProvider : ABlobContainerBasedProvider<Material>
    {
        public MaterialProvider(string containerURL) : base(containerURL)
        {
        }

        protected override Material Convert(byte[] bytes)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
            Material[] objs = assetBundle.LoadAllAssets<Material>();
            assetBundle.Unload(false);
            return objs != null && objs.Length > 0 ? objs[0] : null;
        }
    }
}