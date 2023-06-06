using UnityEngine;

namespace _1.Scripts.Coin
{
    public class CoinCollision : MonoBehaviour
    {
        private bool pickedUp;
    
    
        private void OnTriggerEnter(Collider other)
        {
            if (this.pickedUp)
            {
                return;
            }
        
            if (other.gameObject.layer == LayerMask.NameToLayer("Base Char"))
            {
                GetComponent<PickUpCoin>().PickUp();

                this.pickedUp = true;
            }
        }
    }
}