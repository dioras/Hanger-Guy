using UnityEngine;

namespace _1.Scripts.Rope
{
    public class RopeFollow : MonoBehaviour
    {
        [SerializeField] private Transform followTransform;

        

        private void Update()
        {
            var position = this.followTransform.position;
            position.z = 0f;
            this.transform.position = position;
        }
    }
}