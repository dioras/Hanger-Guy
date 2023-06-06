using UnityEngine;

namespace _1.Scripts.Perks
{
	public abstract class Perk: MonoBehaviour
	{
		public bool IsActive { get; protected set; }
	
	
	
	
		protected virtual void Awake()
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void OnDestroy()
		{
		}


		public abstract void Activate();
		public abstract void Stop();
	}
}