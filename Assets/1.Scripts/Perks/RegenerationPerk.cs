using System.Collections;
using UnityEngine;
using _1.Scripts.Champion;
using _1.Scripts.GameEvent;
using _1.Scripts.Skins;

namespace _1.Scripts.Perks
{
	[RequireComponent(typeof(ChampionHealth))]
	public class RegenerationPerk: Perk
	{
		[SerializeField] private float regenTime;
		[SerializeField] private GameObject regenVfxPrefab;
		private ChampionHealth championHealth;
		private int needRestoreHealthAmount;
		private Coroutine regenerationCor;
		private ParticleSystem regenVfx;




		protected override void Awake()
		{
			base.Awake();

			this.championHealth = GetComponent<ChampionHealth>();
			var regenVfxObj = Instantiate(this.regenVfxPrefab, this.transform);
			regenVfxObj.transform.localPosition = new Vector3(0f, 0.5f, 0f);
			
			this.regenVfx = regenVfxObj.GetComponent<ParticleSystem>();
		}




		public override void Activate()
		{
			if (IsActive)
			{
				return;
			}
		
			IsActive = true;
			
			this.regenerationCor = StartCoroutine(RegenerationProcess());

			EventRepository.ChampionHealthChanged.AddListener(OnChampionHealthChanged);
		}

		public override void Stop()
		{
			if (!IsActive)
			{
				return;
			}

			IsActive = false;
		
			if (this.regenerationCor != null)
			{
				StopCoroutine(this.regenerationCor);

				this.regenerationCor = null;
			}

			this.needRestoreHealthAmount = 0;
			
			EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
		}

		private void PlayRegenVfx()
		{
			this.regenVfx.Play();
		}

		private IEnumerator RestoreHealthProcess(float time, int healthAmount)
		{
			yield return new WaitForSeconds(time);

			this.needRestoreHealthAmount += healthAmount;
		}

		private IEnumerator RegenerationProcess()
		{
			while (true)
			{
				while (this.needRestoreHealthAmount > 0)
				{
					if (this.championHealth.Health <= 0)
					{
						yield break;
					}

					this.championHealth.AddHealth(1);
					
					PlayRegenVfx();

					this.needRestoreHealthAmount--;

					yield return null;
				}

				yield return null;
			}
		}
		
		private void OnChampionHealthChanged(ChampionHealth championHealth, int health, int prevHealth)
		{
			if (this.championHealth == championHealth)
			{
				if (health > 0 && health < championHealth.ChampionPartsNum)
				{
					if (prevHealth > championHealth.ChampionPartsNum)
					{
						prevHealth = championHealth.ChampionPartsNum;
					}

					if (health < prevHealth)
					{
						StartCoroutine(RestoreHealthProcess(this.regenTime, prevHealth - health));
					}
				}
			}
		}
	}
}