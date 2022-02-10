using System;
using UnityEngine;

namespace Atomata {
    public class AtomataMonoControls : MonoBehaviour
    {
        private bool transformSet = false;
        
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;

        void Start() => SetTransform();
        
        public void SetTransform()
        {
            transformSet = true;
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
            originalScale = transform.localScale;
        }

        public void ResetTransform()
        {
            if(!transformSet) SetTransform();
            
            var trans = transform;
            trans.position = originalPosition;
            trans.rotation = originalRotation;
            trans.localScale = originalScale;
        }
    }
}