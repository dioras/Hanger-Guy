using System;
using System.Collections.Generic;
using System.Linq;
using _1.Scripts.Champion;
using UnityEngine;

namespace _1.Scripts.Level.Holes
{
    public class HoleCollisions : MonoBehaviour
    {
        [SerializeField] private float forceFactor;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ChampionHealth>(out var championHealth))
            {
                var partsNum = championHealth.ChampionPartsNum;
            
                championHealth.TakeDamage(100);
                
                for (var i = 0; i < partsNum; i++)
                {
                    foreach (var part in championHealth.ChampionParts[i].Parts)
                    {
                        part.Body.GetComponent<Rigidbody>().AddForce(Vector3.up * this.forceFactor, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}