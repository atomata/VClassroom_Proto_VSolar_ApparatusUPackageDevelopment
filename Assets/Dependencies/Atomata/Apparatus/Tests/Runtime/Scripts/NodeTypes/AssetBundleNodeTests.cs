using UnityEngine;
using NUnit.Framework;
using HexUN.Engine.Utilities;
using HexCS.Core;
using System.Linq;
using HexUN.Framework.Debugging;
using UnityEngine.TestTools;
using System.Collections;
using Cysharp.Threading.Tasks;
using HexUN.Framework.Testing;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Atomata.VSolar.Apparatus.Tests
{
    [TestFixture]
    public class AssetBundleNodeTests : MonoBehaviour
    {
        [OneTimeSetUp]
        public void OneTimeSetup() => UTTests.SetupHexServices();

        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void TearDown()
        {
            //Destroy(par);
        }


        //[UnityTest]
        //public IEnumerator Test1()
        //{
        //    ApparatusAssetNode node = new GameObject().AddComponent<ApparatusAssetNode>();
        //    node.Identifier = "earth";

        //    //yield return node.Load().ToCoroutine();

        //    Assert.That(true);
        //}
    }
}