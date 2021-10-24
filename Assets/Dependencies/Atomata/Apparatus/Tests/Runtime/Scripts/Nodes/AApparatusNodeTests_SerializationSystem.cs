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
using HexUN.Framework.Testing;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Atomata.VSolar.Apparatus.Tests
{
    [TestFixture]
    public class AApparatusNodeTests_SerializationSystem : MonoBehaviour
    {
        [OneTimeSetUp]
        public void OneTimeSetup() => UTTests.SetupHexServices();

        [Test]
        public void Works()
        {
            TestNode_SerializationSystem node = new GameObject().AddComponent<TestNode_SerializationSystem>();
            node.Identifier = "test";
            SrApparatusNode id = node.SerializableId();
            bool idIsCorrect = id.Identifier == node.Identifier && id.Type == node.Type;
            UTTests.Log("The id is correctly serialized", idIsCorrect);

            TestNode_SerializationSystem nodeNew = new GameObject().AddComponent<TestNode_SerializationSystem>();
            nodeNew.Deserialize(id, new string[] { });
            bool idIsDeserializeCorrect = nodeNew.Identifier == id.Identifier;
            UTTests.Log("The serialized id populates correctly", idIsDeserializeCorrect);

            Assert.That(idIsCorrect && idIsDeserializeCorrect);
        }

        public class TestNode_SerializationSystem : AApparatusNode
        {
            public override EApparatusNodeType Type => default;

            protected override UniTask TriggerNode(ApparatusTrigger trigger)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}