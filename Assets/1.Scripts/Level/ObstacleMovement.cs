using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _1.Scripts.Level
{
    public class ObstacleMovement : MonoBehaviour
    {
        [SerializeField] private List<Transform> path;
        [SerializeField] private float deltaStep = 1f;
        [SerializeField] private float minDistance = .1f;

        private List<Vector3> _positions;



        private void Awake()
        {
            if (this.path.Count == 0)
            {
                Destroy(this);
                return;
            }

            this._positions = new List<Vector3>();
            this._positions.AddRange(this.path.Select(c => c.position));
        }

        private void Start()
        {
            StartCoroutine(Movement());
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!this.path.Any())
            {
                return;
            }
            
            Handles.color = Color.red;
            
            if (Application.isPlaying)
            {
                Handles.DrawLine(this._positions[0], this._positions.Last());

                for (var i = 0; i < this._positions.Count - 1; i++)
                {
                    Handles.DrawLine(this._positions[i], this._positions[i + 1]);
                }
            }
            else
            {
                Handles.DrawLine(this.path.Last().position, this.path[0].position);
                
                for (var i = 0; i < this.path.Count - 1; i++)
                {
                    Handles.DrawLine(this.path[i].position, this.path[i + 1].position);
                }
            }
        }
#endif

        
        
        private IEnumerator Movement()
        {
            Vector3 nextTarget;
            var tr = this.transform;
            
            while (true)
            {
                nextTarget = this._positions.First();
                this._positions.Remove(nextTarget);
                this._positions.Add(nextTarget);

                while (Vector3.Distance(tr.position, nextTarget) > this.minDistance)
                {
                    tr.position =
                        Vector3.MoveTowards(tr.position, nextTarget, this.deltaStep * Time.deltaTime);

                    yield return null;
                }

                tr.position = nextTarget;
                yield return null;
            }
        }
    }
}