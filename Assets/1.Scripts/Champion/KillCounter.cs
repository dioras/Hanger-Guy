using UnityEngine;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Champion
{
	public class KillCounter: MonoBehaviour
	{
		public int Kills { get; set; }
		
		
		
	
		private void Awake()
		{
			EventRepository.OnCollisionWithCharacter?.AddListener(OnCollisionWithCharacter);
		}

		private void OnDestroy()
		{
			EventRepository.OnCollisionWithCharacter?.RemoveListener(OnCollisionWithCharacter);
		}




		private void OnCollisionWithCharacter()
		{
			Kills++;
		}
	}
}