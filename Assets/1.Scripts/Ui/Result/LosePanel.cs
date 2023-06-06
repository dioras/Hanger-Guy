using UnityEngine;
using _1.Scripts.Champion;
using _1.Scripts.Game;

namespace _1.Scripts.Ui.Result
{
	public class LosePanel: MonoBehaviour
	{
		[SerializeField] 
		private float invincibilityDurationOnRespawn;
		[SerializeField] 
		private int partsNumOnRespawn;
		[SerializeField] 
		private float startAlphaOnRespawn;
		[SerializeField] 
		private float targetAlphaOnRespawn;
		[SerializeField] 
		private float alphaTransitionTime;
	
	
	
		public void OnClickContinueButton()
		{
			AdsPanel.Instance.ShowRewardedAd("RewardAds_ContinueLevel", RewardAction, AdsPanel.Instance.HidePanel);
		}

		public void SetActive(bool active)
		{
			this.gameObject.SetActive(active);
		}

		private void RewardAction()
		{
			FindObjectOfType<Respawner>().Respawn(this.partsNumOnRespawn, this.invincibilityDurationOnRespawn);
			FindObjectOfType<ChampionMaterial>().ChangeTransparencySmoothly(this.invincibilityDurationOnRespawn, 
				this.startAlphaOnRespawn, this.targetAlphaOnRespawn, this.alphaTransitionTime);
			AdsPanel.Instance.HidePanel();
			FindObjectOfType<GameProcess>().ApplyGameState(GameStateEnum.Continue);
		}
	}
}