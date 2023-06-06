using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _1.Scripts.Level.CharacterObstacle
{
    public class ChangeColliders : MonoBehaviour
    {
        public List<Collider> collidersCharObst;
        // Start is called before the first frame update
        void Start()
        {
            foreach (var item in collidersCharObst)
            {
                item.isTrigger = true;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Champion.Champion>(out var champion) && gameObject.layer != 15)
            {
                foreach(var item in collidersCharObst)
                {
                    item.isTrigger = false;
                }
            }
        }
    }
}
