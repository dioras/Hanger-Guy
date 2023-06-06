using System;
using System.Collections;
using UnityEngine;

namespace _1.Scripts.Perks
{
	public class Magnet<T>: MonoBehaviour where T: Component
	{
		public Action<T> OnMagnetized { get; set; } 
	
		[field:SerializeField] public float Radius { get; set; }
		[field:SerializeField] public float MagnetizationTime { get; set; }
		private T[] objects;




		private void Awake()
		{
			this.objects = FindObjectsOfType<T>();
		}

		private void Update()
		{
			for (var i = 0; i < this.objects.Length; i++)
			{
				if (ReferenceEquals(this.objects[i], null))
				{
					continue;
				}
			
				if (Vector3.Distance(this.transform.position, this.objects[i].transform.position) <= Radius)
				{
					StartCoroutine(Magnetization(this.objects[i], MagnetizationTime));
				
					this.objects[i] = null;
				}
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(this.transform.position, Radius);
		}


		private IEnumerator Magnetization(T obj, float time)
		{
			var currTime = time;

			while (currTime > 0)
			{
				var lerp = 1 - currTime / time;

				if (obj == null)
				{
					yield break;
				}

				obj.transform.position = Vector3.Lerp(obj.transform.position, this.transform.position, lerp);
			
				currTime -= Time.deltaTime;

				yield return null;
			}
			
			OnMagnetized?.Invoke(obj);
		}
	}
}