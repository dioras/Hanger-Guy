using UnityEngine;

namespace _1.Scripts.Helper
{
    public class DestroyWithDelay : MonoBehaviour
    {
        [SerializeField] private float delay = 1.5f;

        

        private void OnEnable()
        {
            Destroy(this.gameObject, this.delay);
        }
    }
}