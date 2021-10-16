using HexCS.Core;

using System.Collections.Generic;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class ApparatusTriggerCarriage
    {
        /// <summary>
        /// The trigger object
        /// </summary>
        public ApparatusTrigger Trigger;

        /// <summary>
        /// Is this a global trigger that applies to everyone
        /// </summary>
        public bool IsGlobal;

        /// <summary>
        /// The next id in the path
        /// </summary>
        public string Next => RemainingPath.Peek();

        /// <summary>
        /// A path queue that must be depleted for the trigger to be activated
        /// </summary>
        public Queue<string> RemainingPath;

        public ApparatusTriggerCarriage(ApparatusTrigger trigger)
        {
            Trigger = trigger;

            if(trigger.Path == "*")
            {
                IsGlobal = true;
            }
            else
            {
                PathString path = trigger.Path;
                RemainingPath = path.AsQueue();
            }
        }

        /// <summary>
        /// Is the id provided the target of the trigger?
        /// </summary>
        public bool IsTarget(string id)
        {
            if (IsGlobal) return true;
            if (RemainingPath.Count != 1) return false;
            if (RemainingPath.Peek() == id) return true;
            return false;
        } 

        /// <summary>
        /// Tries to move to the next path segment. If true is returned, 
        /// the carriage is set up to be received by the next node in the tree. 
        /// If false is returned, then no path remains and the carriage and trigger
        /// should be discarded
        /// </summary>
        public bool MoveToNext()
        {
            if (RemainingPath.Count <= 1) return false;
            RemainingPath.Dequeue();
            return true;
        }
    }
}