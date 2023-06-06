using System.Collections;
using UnityEngine;

namespace _1.Scripts.Ui
{
	public class SplashScreenUI: MonoBehaviour
	{
		private static bool Shown;

		[SerializeField] private float duration;




		private void Awake()
		{
			if (Shown)
			{
				this.gameObject.SetActive(false);
			}
		}

		private void Start()
		{
			StartCoroutine(DisableAfterTime(this.duration));
		}


		private IEnumerator DisableAfterTime(float time)
		{
			yield return new WaitForSeconds(time);

			Shown = true;
			
			this.gameObject.SetActive(false);
		}
	}
}