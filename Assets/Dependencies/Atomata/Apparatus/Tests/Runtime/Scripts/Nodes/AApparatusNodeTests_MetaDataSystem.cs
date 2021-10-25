using UnityEngine;
using NUnit.Framework;
using HexUN.Framework.Debugging;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using System.Collections;
using Cysharp.Threading.Tasks;
using HexUN.Engine.Utilities;
using System.Collections.Generic;
using System.Linq;
using HexCS.Core;
using HexUN.Framework.Testing;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Atomata.VSolar.Apparatus.Tests
{
    [TestFixture]
    public class AApparatusNodeTests_MetaDataSystem : MonoBehaviour
    {
        [OneTimeSetUp]
        public void OneTimeSetup() => UTTests.SetupHexServices();

        [Test]
        public void Works()
        {
            // Setup
            TestNode_MetaData par = new GameObject("par").AddComponent<TestNode_MetaData>();
            par.Identifier = "par";

            TestNode_MetaData ch = par.AddChild<TestNode_MetaData>("ch");
            ch.Identifier = "ch";
            ch.OtherData.Add("custom", "value");

            par.Connect();

            // Tests
            SrApparatusMetadata meta = par.GetMetadata();
            bool pathsAreCorrect = meta.Paths.Length == 2 && meta.Paths.Contains("par") && meta.Paths.Contains("par/ch");
            UTTests.Log("The parent meta data provides expected paths", pathsAreCorrect);


            int parIndex = meta.Paths.QueryIndexOf(s => s == "par");
            int chIndex = meta.Paths.QueryIndexOf(s => s == "par/ch");

            bool valuesAreCorrect = meta.Data.Length == 3
                && meta.Data.Contains($"{parIndex}@info:identifier/par")
                && meta.Data.Contains($"{chIndex}@info:identifier/ch")
                && meta.Data.Contains($"{chIndex}@custom:value");

            UTTests.Log("The parent meta data values are correct", valuesAreCorrect);


            SrApparatusMetadata metaCh = ch.GetMetadata();
            bool chPathsAreCorrect = metaCh.Paths.Length == 1 && metaCh.Paths.Contains("par/ch");
            UTTests.Log("The child meta data provides expected paths", chPathsAreCorrect);

            bool chValuesAreCorrect = meta.Data.Length == 3
                && metaCh.Data.Contains($"0@info:identifier/ch")
                && metaCh.Data.Contains($"0@custom:value");
            UTTests.Log("The child meta data values are correct", chValuesAreCorrect);

            // Assert
            Assert.That(pathsAreCorrect && valuesAreCorrect && chPathsAreCorrect && chValuesAreCorrect);
        }

        public class TestNode_MetaData : AApparatusNode
        {
            public override EApparatusNodeType Type => default;

            public override string NodeType => "Test";

            public Dictionary<string, string> OtherData = new Dictionary<string, string>();

            protected override string[] ResolveMetadata()
            {
                string[] baseData = base.ResolveMetadata();

                string[] newData = new string[OtherData.Count];

                int index = 0;
                foreach (KeyValuePair<string, string> kv in OtherData)
                {
                    newData[index++] = $"{kv.Key}:{kv.Value}";
                }

                return UTArray.Combine(baseData, newData);
            }

            protected override UniTask TriggerNode(ApparatusTrigger trigger)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}