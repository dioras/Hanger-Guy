using UnityEngine;
using _1.Scripts.Champion;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Perks
{
	[RequireComponent(typeof(MagnetPerk))]
	public class MagnetPerkStateController: MonoBehaviour
	{
		private MagnetPerk magnetPerk;
		
		
		
	
		private void Awake()
		{
			this.magnetPerk = GetComponent<MagnetPerk>();
		
			EventRepository.ChampionHealthChanged.AddListener(OnChampionHealthChanged);
		}

		private void OnDestroy()
		{
			EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
		}




		private void OnChampionHealthChanged(ChampionHealth championHealth, int health, int prevHealth)
		{
			if (this.magnetPerk.IsActive)
			{
				if (health <= 1)
				{
					this.magnetPerk.SetActiveMagnet(false);
				}
			}
		}
	}
}