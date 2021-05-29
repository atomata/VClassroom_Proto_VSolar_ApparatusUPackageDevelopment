using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar.Dev
{
    public class DevTrigger_TriggerTestInput : MonoBehaviour
    {
        [SerializeField]
        private ApparatusContainer _container;
        
        public void Input(string inp)
        {
            _container.HandleTrigger(inp);
        }
    }
}