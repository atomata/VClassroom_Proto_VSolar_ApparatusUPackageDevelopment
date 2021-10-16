using HexCS.Core;
using HexUN.Behaviour;
using UnityEngine;

namespace HexUN.Data
{
    /// <summary>
    /// <para>DataProviders classes that provide data through an event and allow
    /// systems to be strung together through generic references, decoupling
    /// implementation of of where data comes from, and the ability ot provide the data. </para>
    /// 
    /// <para>Because C# dosen't allow for double inheritance, it is assumed that dataproviders will
    /// also be monobehaviours. This is necessary to supply them in editor as references</para>
    /// 
    /// <para>Also, this class has no type arguments because a data proivder might provide more than 
    /// one type of data. This means that all listenrs need to do a cast and check when interpreting data</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AMonoDataProvider : HexBehaviour
    {
        protected Event<object> _onProvideData = new Event<object>();

        /// <summary>
        /// The data emitted after filting
        /// </summary>
        public IEventSubscriber<object> OnProvideData => _onProvideData;
    }
}