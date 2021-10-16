using NUnit.Framework;
using HexCS.Core;
using UnityEngine;
using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar
{
    [TestFixture]
    public class ApparatusRequestObjectTests
    {
        [Test]
        public void ApparatusRequestObjectWorks()
        {
            ApparatusRequestObject aro = ApparatusRequestObject.LoadAsset("balls");
            Assert.That(aro.Type == EApparatusRequestType.LoadAsset);

            AssetLoadRequestArgs args = aro.Args as AssetLoadRequestArgs;
            Assert.That(args != null);
        }

        [Test]
        public void ApparatusSaveObjectWorks()
        {
            GameObject proxyObject = new GameObject();
            ApparatusRequestObject aro = ApparatusRequestObject.SaveAsset("Bawlls", proxyObject);
            Assert.That(aro.Type == EApparatusRequestType.SaveAsset);

            AssetSaveRequestArgs args = aro.Args as AssetSaveRequestArgs;
            Assert.That(args != null);
            Assert.That(args.Name == "Bawlls");
            Assert.That(args.Instance == proxyObject);
        }

        [Test]
        public void ApparatusLoadApparatusWorks()
        {
            ApparatusRequestObject aro = ApparatusRequestObject.LoadApparatus("balls");
            Assert.That(aro.Type == EApparatusRequestType.LoadApparatus);

            ApparatusLoadRequestArgs args = aro.Args as ApparatusLoadRequestArgs;
            Assert.That(args != null);
            Assert.That(args.Identifier == "balls");
        }
    }
}
