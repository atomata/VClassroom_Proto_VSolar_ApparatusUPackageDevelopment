using System;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.Apparatus.Runtime.Scripts.Animation
{
    public class AtomataAnimator : MonoBehaviour
    {
        private string lastTrigger = string.Empty;
        
        public Animator Animator;

        public Renderer[] Renderers;
        
        /// <summary>
        /// Triggers only if the cached trigger isn't equal to the trigger
        /// </summary>
        /// <param name="trigger"></param>
        public void TriggerOnce(string trigger)
        {
            if (lastTrigger != trigger)
            {
                Animator.SetTrigger(trigger);
                lastTrigger = trigger;
            }
        }
        
        public void Clear() => lastTrigger = String.Empty;

        
        [ContextMenu("Link")]
        public void LinkRenderers()
        {
            List<Renderer> renders = new List<Renderer>();

            Queue<Transform> process = new Queue<Transform>();
            process.Enqueue(transform);

            while (process.Count > 0)
            {
                Transform target = process.Dequeue();

                foreach (Transform t in target)
                    process.Enqueue(t);

                Renderer rend = target.gameObject.GetComponent<Renderer>();
                
                if(rend != null)
                    renders.Add(rend);
            }

            Renderers = renders.ToArray();
        }

        public void SetRenderersEnabled(bool state)
        {
            foreach (Renderer renderer1 in Renderers)
                renderer1.enabled = state;
        }
    }
}
