using System;
using UnityEngine;

namespace _1.Scripts._Tests.Champion
{
    public class DestroyOnCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Destroy(this.gameObject);
        }
    }
}