using _1.Scripts.Helper;
using UnityEngine;

namespace _1.Scripts.Champion
{
    public class ChampionPartVfx : MonoBehaviour
    {
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private float scaleFactor = 1f;
        
        


        public void PlayVfx()
        {
            var vfx = Instantiate(this.vfxPrefab, this.transform.position, this.transform.rotation);
            vfx.AddComponent<DestroyWithDelay>();
            vfx.transform.localScale *= this.scaleFactor;
        }
    }
}