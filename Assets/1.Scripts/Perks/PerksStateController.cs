using UnityEngine;
using _1.Scripts.Champion;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Perks
{
	[RequireComponent(typeof(ChampionHealth))]
	public class PerksStateController: MonoBehaviour
	{
		private Perk[] perks;
		private ChampionHealth championHealth;




		private void Awake()
		{
			this.perks = GetComponents<Perk>();
			this.championHealth = GetComponent<ChampionHealth>();
		
			EventRepository.ChampionHealthChanged.AddListener(ChampionHealthChanged);
		}

		private void OnDestroy()
		{
			EventRepository.ChampionHealthChanged.RemoveListener(ChampionHealthChanged);
		}



		private void ChampionHealthChanged(ChampionHealth championHealth, int health, int prevHealth)
		{
			if (this.championHealth == championHealth)
			{
				if (prevHealth == 0 && health > 0)
				{
					foreach (var perk in this.perks)
					{
						if (perk.IsActive)
						{
							perk.Stop();
							perk.Activate();
						}
					}
				}
			}
		}
	}
}