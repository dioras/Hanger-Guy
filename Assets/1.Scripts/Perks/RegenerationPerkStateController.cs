using UnityEngine;
using _1.Scripts.Champion;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Perks
{
	[RequireComponent(typeof(RegenerationPerk))]
	public class RegenerationPerkStateController: MonoBehaviour
	{
		private RegenerationPerk regenerationPerk;
		
		
		
	
		private void Awake()
		{
			this.regenerationPerk = GetComponent<RegenerationPerk>();
		
			EventRepository.ChampionHealthChanged.AddListener(OnChampionHealthChanged);
		}

		private void OnDestroy()
		{
			EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
		}




		private void OnChampionHealthChanged(ChampionHealth championHealth, int health, int prevHealth)
		{
			if (health == 0)
			{
				this.regenerationPerk.Stop();
			}

			if (health >= championHealth.ChampionPartsNum)
			{
				if (!this.regenerationPerk.IsActive)
				{
					this.regenerationPerk.Activate();
				}
			}
		}
	}
}