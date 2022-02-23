using System.Collections;
using System.Collections.Generic;
using Atomata.Apparatus.Runtime.Scripts.Animation;
using UnityEngine;

public class AnimatorTriggerer : MonoBehaviour
{
    public AtomataAnimator[] Animator;

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
