using System;
using UnityEngine;
using UnityEngine.UI;
using _1.Scripts.Ads;

namespace _1.Scripts.Ui
{
	public class AdsPanel : MonoBehaviour
	{
		public static AdsPanel Instance { get; private set; }
	
		[field:SerializeField]
		public Button CloseAdButton { get; set; }
		
		private Action rewardAction;
		private Action dismissAction;

		[SerializeField]
		private Component adsComp;
		private IAdvertising ads;
		[SerializeField] 
		private GameObject panel;




		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(this);
			}
			else 
			{
				if (Instance != this)
				{
					Destroy(this.gameObject);
				}
                
				return;
			}
		
			if (this.ads == null)
			{
				if (this.adsComp != null)
				{
					if (!this.adsComp.TryGetComponent(out this.ads))
					{
						Debug.LogError("Ads component is not type of " + typeof(IAdvertising).Name);
					}
				}
				else
				{
					Debug.LogError("Ads component is not set.");
				}
			}
		}


		public void ShowRewardedAd(string placement, Action rewardAction = null, Action dismissAction = null)
		{
			if (!this.ads.IsRewardedAdReady(placement))
			{
				return;
			}
			
			this.rewardAction = rewardAction;
			this.dismissAction = dismissAction;
			
			this.ads.RewardAction = this.rewardAction;
			this.ads.DismissAction = this.dismissAction;
			
			this.panel.SetActive(true);
			
			this.ads.ShowRewardedAd(placement, this.rewardAction, this.dismissAction);
			
			#if NO_ADS
			rewardAction?.Invoke();
			#else
			Time.timeScale = 0;
			#endif
		}
		
		public void ShowInterstitialAd(string placement)
		{
			if (this.ads.IsInterstitialAdReady(placement))
			{
				this.ads.ShowInterstitialAd(placement);
			}
		}

		public void HidePanel()
		{
			this.rewardAction = null;
			this.dismissAction = null;
			
			this.ads.RewardAction = null;
			this.ads.DismissAction = null;
			
			this.panel.SetActive(false);
		}
	}
}