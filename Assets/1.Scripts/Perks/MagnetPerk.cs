using System.Collections;
using UnityEngine;
using _1.Scripts.Coin;

namespace _1.Scripts.Perks
{
	public class MagnetPerk: Perk
	{
		[SerializeField] private CoinMagnet magnetPrefab;
		[field:SerializeField] public float Radius { get; set; }
		[field:SerializeField] public float MagnetizationTime { get; set; }
		[field:SerializeField] public Transform MagnetParent { get; set; }
		private CoinMagnet coinMagnet;




		public void Init(Transform magnetParent)
		{
			this.coinMagnet = Instantiate(this.magnetPrefab);
			
			this.coinMagnet.transform.SetParent(magnetParent);
			
			this.coinMagnet.transform.localRotation = Quaternion.identity;
			this.coinMagnet.transform.localPosition = Vector3.zero;
			
			this.coinMagnet.Radius = Radius;
			this.coinMagnet.MagnetizationTime = MagnetizationTime;
		}

		public override void Activate()
		{
			if (IsActive)
			{
				return;
			}

			IsActive = true;

			if (this.coinMagnet == null)
			{
				Init(this.MagnetParent);
			}
			else
			{
				SetActiveMagnet(true);
			}
		}

		public override void Stop()
		{
			if (!IsActive)
			{
				return;
			}

			IsActive = false;
			
			SetActiveMagnet(false);
		}


		public void SetActiveMagnet(bool active)
		{
			this.coinMagnet.gameObject.SetActive(active);
		}
	}
}