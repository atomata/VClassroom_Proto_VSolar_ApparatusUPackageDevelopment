using UnityEngine;

namespace HexUN
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        public Vector3 EulerRotation;

        private void FixedUpdate()
        {
            transform.rotation *= Quaternion.Euler(EulerRotation * Time.fixedDeltaTime);   
        }
    }
}