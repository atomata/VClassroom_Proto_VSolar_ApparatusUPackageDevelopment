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
    public class AApparatusNodeTests_TriggerSystem : MonoBehaviour
    {
        TestNode_TriggerSystem par;

        TestNode_TriggerSystem c1;
        TestNode_TriggerSystem c11;
        TestNode_TriggerSystem c12;

        TestNode_TriggerSystem c2;
        TestNode_TriggerSystem c21;
        TestNode_TriggerSystem c22;

        List<TestNode_TriggerSystem> allNodes = new List<TestNode_TriggerSystem>();

        [OneTimeSetUp]
        public void OneTimeSetup() => UTTests.SetupHexServices();

        [SetUp]
        public void Setup()
        {
            LogWriter log = new LogWriter("Test");

            par = new GameObject("par").AddComponent<TestNode_TriggerSystem>();
            c1 = par.AddChild<TestNode_TriggerSystem>("c1");
            c11 = c1.AddChild<TestNode_TriggerSystem>("c11");
            c12 = c1.AddChild<TestNode_TriggerSystem>("c12");
            c2 = par.AddChild<TestNode_TriggerSystem>("c2");
            c21 = c2.AddChild<TestNode_TriggerSystem>("c21");
            c22 = c2.AddChild<TestNode_TriggerSystem>("c22");

            par.Identifier = "par";
            c1.Identifier = "c1";
            c11.Identifier = "c11";
            c12.Identifier = "c12";
            c2.Identifier = "c2";
            c21.Identifier = "c21";
            c22.Identifier = "c22";

            par.Connect(log);

            allNodes = new List<TestNode_TriggerSystem>() { par, c1, c11, c12, c2, c21, c22 };
        }

        [TearDown]
        public void TearDown()
        {
            Destroy(par.gameObject);
            allNodes.Clear();
        }

        [UnityTest]
        public IEnumerator LoadTrigger_All()
        {
            LogWriter ignore = new LogWriter("");

            ApparatusTrigger trig = ApparatusTrigger.LoadTrigger(true);
            yield return par.Trigger(trig, ignore).ToCoroutine();

            bool allNodesGotTrigger = allNodes.All(n => n.LastTrigger == null ? false : n.LastTrigger.Equals(trig));
            UTTests.Log("All nodes recieve the trigger", allNodesGotTrigger);

            bool allNodesCalledLoad = allNodes.All(n => n.wasLoadCalled);
            UTTests.Log("All nodes called the load function", allNodesGotTrigger);

            Assert.That(allNodesGotTrigger && allNodesCalledLoad);
        }

        [UnityTest]
        public IEnumerator LoadTrigger_One()
        {
            LogWriter ignore = new LogWriter("");

            ApparatusTrigger trig = ApparatusTrigger.LoadTrigger(true, c22.Path());
            yield return par.Trigger(trig, ignore).ToCoroutine();
            bool c22Triggered = c22.LastTrigger != null && c22.LastTrigger.Equals(trig) && c22.wasLoadCalled;
            UTTests.Log("child was triggered", c22Triggered);

            ApparatusTrigger trig2 = ApparatusTrigger.LoadTrigger(true, par.Path());
            yield return par.Trigger(trig2, ignore).ToCoroutine();
            bool parTriggered = par.LastTrigger != null && par.LastTrigger.Equals(trig2) && par.wasLoadCalled;
            UTTests.Log("parent was triggered", parTriggered);

            Assert.That(c22Triggered && parTriggered);
        }

        [UnityTest]
        public IEnumerator LoadTrigger_FalsePath()
        {
            LogWriter ignore = new LogWriter("");

            ApparatusTrigger trig = ApparatusTrigger.LoadTrigger(true, c22.Path().InsertAtEnd("fake"));
            yield return par.Trigger(trig, ignore).ToCoroutine();
            bool noneTriggered = !allNodes.Any(n => n.wasLoadCalled || n.LastTrigger != null);
            UTTests.Log("No node was triggered", noneTriggered);

            Assert.That(noneTriggered);
        }

        public class TestNode_TriggerSystem : AApparatusNode
        {
            public override string NodeType => "Test";

            public ApparatusTrigger LastTrigger = null;

            public bool wasLoadCalled;

            protected async override UniTask TriggerNode(ApparatusTrigger trigger, LogWriter log)
            {
                wasLoadCalled = false;
                LastTrigger = trigger;

                if (trigger.Type == ETriggerType.Load) await Load();

                return;
            }

            protected override string[] ResolveMetadata()
            {
                throw new System.NotImplementedException();
            }

            protected async UniTask Load()
            {
                wasLoadCalled = true;
            }
        }
    }
}