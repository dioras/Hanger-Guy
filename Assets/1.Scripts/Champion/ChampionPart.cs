using System;
using System.Collections;
using UnityEngine;

namespace _1.Scripts.Champion
{
    public class ChampionPart : MonoBehaviour
    {
        public Action<ChampionPart> OnDetached { get; set; }
        public bool IsDetached { get; private set; }
        public GameObject Body => this.body;

        [SerializeField] private GameObject body;
        [SerializeField] private Rigidbody championRb;
        [SerializeField] private GameObject toDisable;
        [SerializeField] private float restoreTime;
        private Coroutine restoreCor;



        public void Detach()
        {
            if (IsDetached)
            {
                return;
            }
            
            this.body.SetActive(true);
            this.gameObject.SetActive(false);
            
            var bodyRigidbody = this.body.GetComponent<Rigidbody>();
            
            bodyRigidbody.velocity = this.championRb.velocity;
            
            this.body.transform.position = this.transform.position;
            this.body.transform.rotation = this.transform.rotation;
            this.body.transform.localScale = this.transform.localScale;
            
            var vfx = this.body.GetComponent<ChampionPartVfx>();
            if (vfx != null)
            {
                vfx.PlayVfx();
            }

            if (this.restoreCor != null)
            {
                StopCoroutine(this.restoreCor);

                this.restoreCor = null;
            }
            
            IsDetached = true;
            
            OnDetached?.Invoke(this);
        }
        
        public void Attach()
        {
            this.body.SetActive(false);
            this.gameObject.SetActive(true);
            
            if (this.restoreCor != null)
            {
                StopCoroutine(this.restoreCor);

                this.restoreCor = null;
            }

            this.restoreCor = StartCoroutine(RestorePartAnim(this.restoreTime));
            
            IsDetached = false;
        }

        private IEnumerator RestorePartAnim(float time)
        {
            var startScale = Vector3.zero;
            var targetScale = Vector3.one;
        
            var currTime = time;

            while (currTime > 0)
            {
                var lerp = 1 - currTime / time;
                
                this.toDisable.transform.localScale = Vector3.Lerp(startScale, targetScale, lerp);

                currTime -= Time.deltaTime;

                yield return null;
            }

            this.toDisable.transform.localScale = targetScale;

            this.restoreCor = null;
        }
    }
}