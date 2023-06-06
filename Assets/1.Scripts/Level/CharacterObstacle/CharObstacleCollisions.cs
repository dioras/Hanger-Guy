using _1.Scripts.GameEvent;
using UnityEngine;

namespace _1.Scripts.Level.CharacterObstacle
{
    public class CharObstacleCollisions : MonoBehaviour
    {
        [SerializeField] private int coins;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Champion.Champion>(out var champion) && gameObject.layer != 15) 
            {
                GetComponent<CharacterObstacle>().Deactivate(new Vector3(1, 1, 0).normalized);
                GetComponent<LayoutController>().Disable();
                EventRepository.CoinPickedUp.Invoke(this.coins);
                EventRepository.OnCollisionWithCharacter?.Invoke();
            }
        }
    }
}