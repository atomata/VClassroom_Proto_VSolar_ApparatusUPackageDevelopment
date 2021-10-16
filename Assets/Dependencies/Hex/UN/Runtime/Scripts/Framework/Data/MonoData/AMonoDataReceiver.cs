using HexCS.Core;
using HexUN.Behaviour;
using UnityEngine;

namespace HexUN.Data
{
    /// <summary>
    /// <para>
    /// MonoData receivers automatically attempt to find all 
    /// AMonoDataProividers on the gameobject they're attached to,
    /// subscribe to the data event and provide a function for interpreting
    /// incoming data
    /// </para>
    /// </summary>
    public abstract class AMonoDataReciever : HexBehaviour
    {
        protected override void HexAwake()
        {
            AMonoDataProvider[] providers = gameObject.GetComponents<AMonoDataProvider>();

            foreach (AMonoDataProvider p in providers)
            {
                EventBindings.Add( p.OnProvideData.Subscribe(HandleDataReceived) );
            }
        }

        /// <summary>
        /// How to handle data received
        /// </summary>
        /// <param name="data"></param>
        protected abstract void HandleDataReceived(object data);
    }
}