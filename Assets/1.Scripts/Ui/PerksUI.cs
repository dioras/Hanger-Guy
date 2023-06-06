using System.Linq;
using UnityEngine;
using _1.Scripts.Game;
using _1.Scripts.Perks;
using _1.Scripts.Skins;

namespace _1.Scripts.Ui
{
	public class PerksUI: MonoBehaviour
	{
		[SerializeField] private GameObject[] objectsForDisable;
		[SerializeField] private FollowCamera followCamera;
	
		public void OnClickMagnetCoin()
		{
			AdsPanel.Instance.ShowRewardedAd("RewardAds_Magnit", MagnetCoinRewardAction, AdsPanel.Instance.HidePanel);
		}

		public void OnClickIronGuy()
		{
			AdsPanel.Instance.ShowRewardedAd("RewardAds_Invulnerability", IronGuyRewardAction, AdsPanel.Instance.HidePanel);
		}

		public void OnClickRegenReptile()
		{
			AdsPanel.Instance.ShowRewardedAd("RewardAds_Regeneration", RegenReptileRewardAction, AdsPanel.Instance.HidePanel);
		}
		
		private void MagnetCoinRewardAction()
		{
			var magnetPerk = FindObjectOfType<Champion.Champion>().GetComponent<MagnetPerk>();
			
			magnetPerk.Activate();
			
			DisableObjects();
		
			AdsPanel.Instance.HidePanel();
		}
		
		private void IronGuyRewardAction()
		{
			var skin = SkinRepository.Instance.Skins.Single(s => s.Name.Equals("IronMan"));
		
			SkinChanger.Instance.SetSkin(skin);

			var skinBody = SkinChanger.Instance.GetSkinBody(skin);

			this.followCamera.Target = skinBody.transform;
		
			var invulnerabilityPerk = skinBody.GetComponent<InvulnerabilityPerk>();
			
			invulnerabilityPerk.Activate();
			
			DisableObjects();
		
			AdsPanel.Instance.HidePanel();
		}

		private void RegenReptileRewardAction()
		{
			var skin = SkinRepository.Instance.Skins.Single(s => s.Name.Equals("Deadpool"));
		
			SkinChanger.Instance.SetSkin(skin);
			
			var skinBody = SkinChanger.Instance.GetSkinBody(skin);

			this.followCamera.Target = skinBody.transform;
		
			var regenerationPerk = skinBody.GetComponent<RegenerationPerk>();
			
			regenerationPerk.Activate();
			
			DisableObjects();
		
			AdsPanel.Instance.HidePanel();
		}

		private void DisableObjects()
		{
			foreach (var obj in this.objectsForDisable)
			{
				obj.SetActive(false);
			}
		}
	}
}