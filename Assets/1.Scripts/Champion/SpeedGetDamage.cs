using UnityEngine;

namespace _1.Scripts.Champion
{
    public class SpeedGetDamage : MonoBehaviour, IGetDamage
    {
        [SerializeField] private float speedFactor;

        private Rigidbody _rb;
        
        
        
        public int GetDamage()
        {
            return (int)(this._rb.velocity.magnitude * this.speedFactor);
        }


        
        private void Awake()
        {
            this._rb = GetComponent<Rigidbody>();
        }
    }
}