using UnityEngine;

namespace _1.Scripts.Vfxs
{
    public class VfxActivator : MonoBehaviour
    {
        [SerializeField] private GameObject vfx;
        [SerializeField] private Transform root;
        [SerializeField] private float destroyDelay;
        
        



        public void Activate()
        {
            vfx.transform.parent = root;
            vfx.SetActive(true);
            Destroy(vfx, destroyDelay);
        }
    }
}