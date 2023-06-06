using UnityEngine;

namespace _1.Scripts.Level.CharacterObstacle
{
    public class LayoutController : MonoBehaviour
    {
        [SerializeField] private float minSpeed;
        [SerializeField] private Rigidbody body;




        private void Awake()
        {
            StoneEnemy();
        }

        private void FixedUpdate()
        {
            if (!this.body)
            {
                return;
            }
            
            if (this.body.velocity.magnitude >= minSpeed && gameObject.layer != 0)
            {
                EnableChampionEnemy();
            }
            else if (this.body.velocity.magnitude < minSpeed && gameObject.layer != 14)
            {
                StoneEnemy();
            }
        }




        public void SetBody(Rigidbody body)
        {
            this.body = body;
        }
        
        public void Disable()
        {
            ChangeLayout(transform, 14);
            this.enabled = false;
            Destroy(this);
        }

        private void ChangeLayout(Transform tr, int layout)
        {
            tr.gameObject.layer = layout;
            
            foreach (Transform child in tr)
            {
                ChangeLayout(child, layout);
            }
        }

        private void DisableChampionEnemy()
        {
            ChangeLayout(transform, 14);
        }

        private void EnableChampionEnemy()
        {
            ChangeLayout(transform, 0);
        }

        private void StoneEnemy()
        {
            ChangeLayout(transform, 15);
        }
    }
}