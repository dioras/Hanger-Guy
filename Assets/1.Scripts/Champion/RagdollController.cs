using System.Collections.Generic;
using UnityEngine;

namespace _1.Scripts.Champion
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] private Collider rootCollider;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private List<Rigidbody> rigidbodies;
        [SerializeField] private float forceFactor;
        
        

        public void EnableRagdoll(bool state)
        {
            animator.enabled = !state;
            rootCollider.enabled = !state;

            rb.useGravity = state;
            rb.isKinematic = !state;
            
            foreach (var rb in rigidbodies)
            {
                rb.useGravity = state;
                rb.isKinematic = !state;
            }
        }

        public void AddForce(Vector3 direction)
        {
            rb.AddForce(direction * forceFactor, ForceMode.Impulse);
            
            foreach (var rb in rigidbodies)
            {
                rb.AddForce(direction * forceFactor, ForceMode.Impulse);
            }
        }

        

        private void Awake()
        {
            EnableRagdoll(false);
        }
    }
}