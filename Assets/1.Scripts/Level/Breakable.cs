using UnityEngine;

namespace _1.Scripts.Level
{
	public class Breakable: MonoBehaviour
	{
		[SerializeField] private Rigidbody[] rigidbodies;
		[SerializeField] private Behaviour[] disableBehavioursOnBreak;
		[SerializeField] private GameObject[] disableObjectsOnBreak;
		[SerializeField] private Renderer[] disableRenderersOnBreak;
		[SerializeField] private Collider[] disableCollidersOnBreak;
		public bool IsBroken { get; set; }



		public void Break(Vector3 impulse)
		{
			if (IsBroken)
			{
				return;
			}

			IsBroken = true;
			
			foreach (var behaviour in this.disableBehavioursOnBreak)
			{
				if (behaviour == null)
				{
					continue;
				}
				
				behaviour.enabled = false;
			}

			foreach (var obj in this.disableObjectsOnBreak)
			{
				obj.SetActive(false);
			}
			
			foreach (var renderer in this.disableRenderersOnBreak)
			{
				renderer.enabled = false;
			}

			foreach (var collider in this.disableCollidersOnBreak)
			{
				collider.enabled = false;
			}
		
			foreach (var rigidbody in this.rigidbodies)
			{
				rigidbody.transform.SetParent(null);
				rigidbody.gameObject.SetActive(true);
				rigidbody.isKinematic = false;
				rigidbody.AddForce(impulse, ForceMode.Impulse);
			}
		}
	}
}