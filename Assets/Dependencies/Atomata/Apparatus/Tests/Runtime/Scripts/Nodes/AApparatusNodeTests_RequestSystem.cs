using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Atomata.VSolar.Apparatus;
using HexUN.Engine.Utilities;
using HexCS.Core;
using System.Linq;
using HexUN.Framework.Debugging;
using HexUN.Framework.Request;
using HexUN.Framework.Testing;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Atomata.VSolar.Apparatus.Tests
{
    [TestFixture]
    public class AApparatusNodeTests_RequestSystem : MonoBehaviour
    {
        private TestNode_Requests par;
        private TestNode_Requests ch1;
        private TestNode_Requests ch11;
        private TestNode_Requests ch12;
        private TestNode_Requests ch2;
        private TestNode_Requests ch21;
        private TestNode_Requests ch22;

        [OneTimeSetUp]
        public void OneTimeSetup() => UTTests.SetupHexServices();

        [SetUp]
        public void Setup()
        {
            par = new GameObject("par1").AddComponent<TestNode_Requests>();

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

        [UnityTest]
        public IEnumerator ExampleCoroutineTest()
        {
            // Setup up
            TestNode_Requests par
                = new GameObject("par1").AddComponent<TestNode_Requests>();

            yield return par.LoadMeAnApparatus();
        }


        public class TestNode_Requests : AApparatusNode
        {
            public override EApparatusNodeType Type => default;

            public override string NodeType => "Test";

            public void OnEnable()
            {
                RequestHandler = HandleRequest;
            }

            private void HandleRequest(ApparatusRequest request, LogWriter log)
            {
                if (request.RequestObject.Type == EApparatusRequestType.LoadApparatus)
                {
                    request.RequestObject.TryAs(out ApparatusLoadRequestArgs args);
                    GameObject prefab = new GameObject(args.Identifier);
                    request.Respond(ApparatusResponseObject.AssetResponse(prefab), this);
                }
            }

            public IEnumerator LoadMeAnApparatus()
            {
                LogWriter writer = new LogWriter(nameof(LoadMeAnApparatus));

                ApparatusRequest ar = SendRequest(ApparatusRequestObject.LoadApparatus("testapp"), writer);

                while (ar.State != Request<ApparatusRequestObject, ApparatusResponseObject>.EState.Complete)
                {
                    yield return new WaitForEndOfFrame();
                }

                if (ar.ResponseObject.Status == EApparatusResponseStatus.Success) Debug.Log("The request succeeded");
                else Debug.Log("The request failed");
            }

            protected override UniTask TriggerNode(ApparatusTrigger trigger, LogWriter log)
            {
                throw new System.NotImplementedException();
            }
        }

    }
}