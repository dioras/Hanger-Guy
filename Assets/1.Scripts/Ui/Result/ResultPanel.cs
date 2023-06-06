using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Ui.Result
{
	public class ResultPanel: MonoBehaviour
	{
		public enum State
		{
			Claim, NextLevel
		}
	
		[SerializeField] 
		private Button claimButton;
		[SerializeField] 
		private Button nextLevelButton;
		[SerializeField]
		private Button notNowButton;
		[SerializeField] 
		private float showNotNowTextDelay;
		[SerializeField] 
		private float notNowAlphaTransitionTime;
		private Coroutine showNotNowCoroutine;
		[SerializeField] 
		private TextMeshProUGUI coinsText;
		[SerializeField] 
		private string intAdsNextLevelName;
		[SerializeField] 
		private string rewardAdsX2Name;
		[SerializeField] 
		private float coinsX2AnimTime;

		[SerializeField] private int coinsMultiplier;


		private void Awake()
		{
			EventRepository.GameStateChanged.AddListener(OnGameStateChanged);			
		}

		private void OnGameStateChanged(GameStateEnum gameState)
		{
			switch (gameState)
			{
				case GameStateEnum.Result:
					SetState(MatchCoinCollector.Instance.CoinsAmount > 0 ? State.Claim : State.NextLevel);

					break;
			}
		}
		
		public void OnClickNextLevelButton()
		{
			AdsPanel.Instance.ShowInterstitialAd(this.intAdsNextLevelName);
		}

		public void OnClickClaimButton()
		{
			AdsPanel.Instance.ShowRewardedAd(this.rewardAdsX2Name, ClaimRewardAction, AdsPanel.Instance.HidePanel);
		}

		public void SetState(State state)
		{
			switch (state)
			{
				case State.Claim:
					SetClaimState();
					
					break;
				case State.NextLevel:
					SetNextLevelState();
				
					break;
			}
		}
		
		public void SetClaimState()
		{
			this.coinsText.text = MatchCoinCollector.Instance.CoinsAmount.ToString();
		
			this.claimButton.gameObject.SetActive(true);
			this.nextLevelButton.gameObject.SetActive(false);

			if (this.showNotNowCoroutine != null)
			{
				StopCoroutine(this.showNotNowCoroutine);
			}
			
			this.showNotNowCoroutine = 
				StartCoroutine(ShowNotNowTextWithDelay(this.showNotNowTextDelay, this.notNowAlphaTransitionTime));
		}

		public void SetNextLevelState()
		{
			this.coinsText.text = MatchCoinCollector.Instance.CoinsAmount.ToString();
		
			this.claimButton.gameObject.SetActive(false);
			this.nextLevelButton.gameObject.SetActive(true);
			this.notNowButton.gameObject.SetActive(false);
			if (this.showNotNowCoroutine != null)
			{
				StopCoroutine(this.showNotNowCoroutine);
			}
		}

		private IEnumerator ShowNotNowTextWithDelay(float delay, float time)
		{
			var buttonText = this.notNowButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

			var startColorAlpha = 0f;
			var targetColorAlpha = buttonText.color.a;
		
			var textColor = buttonText.color;

			textColor.a = startColorAlpha;

			buttonText.color = textColor;
		
			this.notNowButton.gameObject.SetActive(true);
		
			this.notNowButton.interactable = false;
		
			yield return new WaitForSeconds(delay);

			var currTime = time;

			while (currTime > 0)
			{
				var lerp = 1 - currTime / time;

				var color = buttonText.color;

				color.a = Mathf.Lerp(startColorAlpha, targetColorAlpha, lerp);

				buttonText.color = color;
			
				currTime -= Time.deltaTime;
				
				yield return null;
			}

			textColor.a = targetColorAlpha;

			buttonText.color = textColor;
			
			this.notNowButton.interactable = true;
		}
		private void ClaimRewardAction()
		{
			var matchCoins = MatchCoinCollector.Instance.CoinsAmount;

			StartCoroutine(AnimCoinsReward(matchCoins, this.coinsX2AnimTime));
		
			var wallet = FindObjectOfType<Wallet.Wallet>();
		
			wallet.AddCoins(matchCoins * (this.coinsMultiplier - 1));
			
			this.coinsText.text = matchCoins.ToString();
			
			SetState(State.NextLevel);
			
			AdsPanel.Instance.HidePanel();
		}

		private IEnumerator AnimCoinsReward(int coins, float time)
		{
			var startCoins = coins;
			var targetCoins = coins * this.coinsMultiplier;

			var currTime = time;

			var stringBuilder = new StringBuilder();

			while (currTime > 0)
			{
				var lerp = 1 - currTime / time;

				var currCoins = Mathf.Lerp(startCoins, targetCoins, lerp);
			
				stringBuilder.Clear();
				stringBuilder.Append(Mathf.RoundToInt(currCoins));
			
				this.coinsText.text = stringBuilder.ToString();
			
				currTime -= Time.deltaTime;

				yield return null;
			}

			this.coinsText.text = targetCoins.ToString();
		}
	}
}