using System.Collections;
using System.Collections.Generic;
using Atomata.Apparatus.Runtime.Scripts.Animation;
using UnityEngine;

public class AnimatorTriggerer : MonoBehaviour
{
    public AtomataAnimator[] Animator;

    /// <summary>
    /// Performs a BFS on game objects and registers all Atomata animators, but not their
    /// children
    /// </summary>
    [ContextMenu("FindAndRegisterAnimators")]
    public void FindAndRegisterAnimators()
    {
        Queue<Transform> s = new Queue<Transform>();
        foreach (Transform t in transform)
            s.Enqueue(t);

        List<AtomataAnimator> animators = new List<AtomataAnimator>();
        while (s.Count > 0)
        {
            Transform process = s.Dequeue();
            
            AtomataAnimator anim = process.gameObject.GetComponent<AtomataAnimator>();
            if (anim != null)
            {
                animators.Add(anim);
                continue;
            }
            
            foreach (Transform t in process)
                s.Enqueue(t);
        }

        Animator = animators.ToArray();
    }
    
    public void AnimEventTriggerSubAnimators(string s)
    {
        string[] inst = s.Split(',');

        foreach (string s1 in inst)
        {
            string[] spl = s1.Split('@');
            int index = int.Parse(spl[0]);


            if (spl[1].StartsWith("rend"))
                Animator[index].SetRenderersEnabled(bool.Parse(spl[1].Split(':')[1]));
            else
                Animator[index].TriggerOnce(spl[1]);
        }
    }
}
