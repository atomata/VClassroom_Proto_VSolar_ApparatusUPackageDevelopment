using System;
using UnityEditor.Animations;
using UnityEngine;

namespace Atomata.Apparatus.Runtime.Scripts.Animation
{
    public class OnlyOnceTriggerer : MonoBehaviour
    {
        private string lastTrigger = string.Empty;

        public Animator Animator;

        public void Trigger(string trigger)
        {
            if (lastTrigger != trigger)
            {
                Animator.SetTrigger(trigger);
                lastTrigger = trigger;
            }
        }

        public void Clear() => lastTrigger = String.Empty;
    }
}
