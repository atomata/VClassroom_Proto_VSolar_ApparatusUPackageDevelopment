using UnityEngine;
using NUnit.Framework;
using HexUN.Engine.Utilities;
using HexCS.Core;
using System.Linq;
using HexUN.Framework.Testing;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Atomata.VSolar.Apparatus.Tests
{
    [TestFixture]
    public class AApparatusNodeTests_ConnectionSystem : MonoBehaviour
    {
        private TestNode_Connections par;
        private TestNode_Connections ch1;
        private TestNode_Connections ch11;
        private TestNode_Connections ch12;
        private TestNode_Connections ch2;
        private TestNode_Connections ch21;
        private TestNode_Connections ch22;

        [OneTimeSetUp]
        public void OneTimeSetup() => UTTests.SetupHexServices();

        [SetUp]
        public void Setup()
        {
            par = new GameObject("par1").AddComponent<TestNode_Connections>();

            par.AddChild("ch1", out ch1);
            ch1.AddChild("ch11", out ch11);
            ch1.AddChild("ch12", out ch12);

            par.AddChild("ch2", out ch2);
            ch2.AddChild("ch21", out ch21);
            ch2.AddChild("ch22", out ch22);
        }

        [TearDown]
        public void TearDown()
        {
            Destroy(par);
        }

        [Test]
        public void Connect()
        {
            par.Connect();
            TestNode_Connections[] nodes = new TestNode_Connections[] { par, ch1, ch2, ch11, ch12, ch21, ch22 };

            // Test the onConnected callback is functioning correctly
            bool OnConnectedCallback = !nodes.QueryContains(n => !n.isConnected);
            UTTests.Log("OnConnected protected node callback", OnConnectedCallback);

            // Test that all the parents are set correctly
            bool AllParentsSet =
                par.Parent == null
                && ch1.Parent == par && ch2.Parent == par
                && ch11.Parent == ch1 && ch12.Parent == ch1
                && ch21.Parent == ch2 && ch22.Parent == ch2;
            UTTests.Log("Parents were set to the correct nodes", AllParentsSet);

            // Test that all the children are set correctly
            bool AllChildrenSet =
                par.Children.Contains(ch1) && par.Children.Contains(ch2)
                && ch1.Children.Contains(ch11) && ch1.Children.Contains(ch12)
                && ch2.Children.Contains(ch21) && ch2.Children.Contains(ch22)
                && ch11.Children.Length == 0 && ch12.Children.Length == 0
                && ch21.Children.Length == 0 && ch22.Children.Length == 0;
            UTTests.Log("Children were set correctly", AllChildrenSet);

            // Assert
            Assert.That(OnConnectedCallback && AllParentsSet && AllChildrenSet);
        }

        [Test]
        public void Disconnect()
        {
            par.Connect();
            TestNode_Connections[] nodes = new TestNode_Connections[] { par, ch1, ch2, ch11, ch12, ch21, ch22 };

            par.Disconnect();

            // Test the on disconnected callback is functioning correctly
            bool OnDisconnectedCallback = nodes.All(n => !n.isConnected); // TO DO: Add on disconnected callback
            UTTests.Log("OnDisconnected callback works", OnDisconnectedCallback);

            // Test that all the parents are set to null
            bool AllParentsRemoved = nodes.All(n => n.Parent == null);
            UTTests.Log("Parents were set to null", AllParentsRemoved);

            // Test that all the children are set to list of 
            bool AllChildrenRemoved = nodes.All(n => n.Children.Length == 0);
            UTTests.Log("Children were removed correctly", AllChildrenRemoved);

            // Assert
            Assert.That(OnDisconnectedCallback && AllParentsRemoved && AllChildrenRemoved);
        }

        [Test]
        public void PropertiesUpdate()
        {
            TestNode_Connections[] nodes = new TestNode_Connections[] { par, ch1, ch2, ch11, ch12, ch21, ch22 };

            bool allNodesDisconnectedOnStart = nodes.All(n => n.ConnectionState == EApparatusNodeConnectionState.Disconnected);
            UTTests.Log("All nodes start off in disconnected state", allNodesDisconnectedOnStart);

            par.Connect();
            bool allNodesBecomeConnected = nodes.All(n => n.ConnectionState == EApparatusNodeConnectionState.Connected);
            UTTests.Log("All nodes become connected after connecting", allNodesBecomeConnected);

            par.Disconnect();
            bool allNodesAreNowDisconnected = nodes.All(n => n.ConnectionState == EApparatusNodeConnectionState.Disconnected);
            UTTests.Log("All nodes become unconnected after disconnected", allNodesAreNowDisconnected);

            Assert.That(allNodesDisconnectedOnStart && allNodesBecomeConnected && allNodesAreNowDisconnected);
        }

        public class TestNode_Connections : AApparatusNode
        {
            public override EApparatusNodeType Type => default;

            public bool isConnected = false;

            protected override void OnConnected() => isConnected = true;

            protected override void OnDisconnected() => isConnected = false;

            protected override UniTask TriggerNode(ApparatusTrigger trigger)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}