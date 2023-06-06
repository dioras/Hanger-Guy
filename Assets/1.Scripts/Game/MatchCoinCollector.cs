using UnityEngine;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Game
{
	public class MatchCoinCollector: MonoBehaviour
	{
		public static MatchCoinCollector Instance { get; private set; }
	
		public int CoinsAmount { get; private set; }
		private Wallet.Wallet Wallet
		{
			get
			{
				if (this.wallet == null)
				{
					this.wallet = FindObjectOfType<Wallet.Wallet>();
				}

				return this.wallet;
			}
		}

		private Wallet.Wallet wallet;
		
	
	
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
		
			EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
			EventRepository.CoinPickedUp.AddListener(CoinPickedUp);
		}

		private void OnDestroy()
		{
			EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
			EventRepository.CoinPickedUp.RemoveListener(CoinPickedUp);
		}



		private void OnGameStateChanged(GameStateEnum gameState)
		{
			switch (gameState)
			{
				case GameStateEnum.Play:
					CoinsAmount = 0;
					break;
				case GameStateEnum.Result:
					Wallet.AddCoins(CoinsAmount);
					break;
			}
		}
		
		private void CoinPickedUp(int coins)
		{
			CoinsAmount += coins;
		}
	}
}