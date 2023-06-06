using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _1.Scripts.Champion
{
    public class MainColliderSize : MonoBehaviour
    {
        [Header("1")]
        [SerializeField] private float centerY1 = .6f;
        [SerializeField] private float height1 = 1.2f;
        [Header("2")]
        [SerializeField] private float centerY2 = .87f;
        [SerializeField] private float height2 = .65f;
        [Header("3")]
        [SerializeField] private float centerY3 = .95f;
        [SerializeField] private float height3 = .5f;
        [Header("Bodies")]
        [SerializeField] private List<GameObject> bodies2;
        [SerializeField] private List<GameObject> bodies3;

        private CapsuleCollider _collider;
        private bool _is2;
        private bool _is3;


        
        private void Awake()
        {
            this._collider = GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            if (!this._is2 && this.bodies2.All(c => !c.activeSelf))
            {
                this._is2 = true;
                
                var center = this._collider.center;
                center.y = this.centerY2;
                this._collider.center = center;
                
                this._collider.height = this.height2;
            }
            else if (!this._is3 && this.bodies3.All(c => !c.activeSelf))
            {
                this._is3 = true;
                
                var center = this._collider.center;
                center.y = this.centerY3;
                this._collider.center = center;
                
                this._collider.height = this.height3;
            }
        }
    }
}