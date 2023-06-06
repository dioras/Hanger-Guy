using _1.Scripts.GameEvent;
using UnityEngine;

namespace _1.Scripts.Coin
{
    public class PickUpCoin : MonoBehaviour
    {
        [SerializeField] private GameObject pickUpVfx;
        [SerializeField] private int coins;
        
        
        public void PickUp()
        {
            ActivateVfx();
            EventRepository.CoinPickedUp.Invoke(this.coins);
            Destroy(this.gameObject);
        }



        private void ActivateVfx()
        {
            this.pickUpVfx.transform.parent = null;
            this.pickUpVfx.gameObject.SetActive(true);
        }
    }
}