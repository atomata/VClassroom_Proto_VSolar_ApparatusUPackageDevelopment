using System.IO;
using Atomata.VSolar.Apparatus.Example;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    // TODO: Abstract an interface or abstract class for multiple platform
    public class WebGLContainerManager : MonoBehaviour
    {
        [SerializeField] private ApparatusContainer _container;
        public ApparatusContainer Container {get => _container;set => _container = value; }


        public string test;

        [ContextMenu("test")]
        public void LoadApparatus()
        { 
            _container.LoadApparatus(test);
        }

        public void UnloadApparatus()
        {
            
        }
        
        
        // Load Apparatus
        // Unload Apparatus
        // SendTrigger
    }
}
