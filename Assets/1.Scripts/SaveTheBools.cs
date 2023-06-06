using UnityEngine;

namespace _1.Scripts
{
	public class SaveTheBools : MonoBehaviour {
 
		public string boolName;
 
		private Animator animator = null;
		private bool boolValue;
 
 
 
		private void Start() 
		{
			this.animator = GetComponent<Animator>();
			if (this.animator != null)
			{
				boolValue = this.animator.GetBool(boolName);
			}
		}
 
		private void OnEnable() 
		{
			if (this.animator != null)
			{
				this.animator.SetBool(boolName, boolValue);
			}
		}

		private void OnDisable() 
		{
			if (this.animator != null)
			{
				boolValue = this.animator.GetBool(boolName);
			}
		}
	}
}