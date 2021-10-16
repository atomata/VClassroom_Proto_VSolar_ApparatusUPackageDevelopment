using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class AnnotationCanvas : MonoBehaviour
    {
        public Camera m_Camera;


        private void Start()
        {
            m_Camera = Camera.main;
        }

        void LateUpdate()
        {
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                m_Camera.transform.rotation * Vector3.up);
        }
    }
}