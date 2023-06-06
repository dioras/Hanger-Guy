using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _1.Scripts.GameEvent;
using _1.Scripts.Skins;

namespace _1.Scripts.Ui.Shop
{
	public class SkinUi: MonoBehaviour
	{
		public enum State
		{
			Selected, NotSelected, CanBuy, Available, NotAvailable, ForAds
		}

		[SerializeField] private Button selectButton;
		[SerializeField] private Image selectedImage;
		[SerializeField] private Image[] skinAvatars;
		[SerializeField] private GameObject availablePanel;
		[SerializeField] private GameObject notAvailablePanel;
		public State CurrentState { get; private set; }
		public Skin Skin { get; private set; }
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



		private void OnEnable()
		{
			if (Skin != null)
			{
				var skinState = GetSkinState(Skin);
			
				SetState(skinState);
			}
		}

		private void Awake()
		{
			EventRepository.WalletCoinsCountChanged.AddListener(OnWalletCoinsCountChanged);
			PlayerSkinService.Instance.OnCurrentSkinChanged += OnCurrentSkinChanged;
			PlayerSkinService.Instance.OnSkinAdded += OnSkinAdded;
		}

		private void OnDestroy()
		{
			EventRepository.WalletCoinsCountChanged.RemoveListener(OnWalletCoinsCountChanged);
			PlayerSkinService.Instance.OnCurrentSkinChanged -= OnCurrentSkinChanged;
			PlayerSkinService.Instance.OnSkinAdded -= OnSkinAdded;
		}

		


		public void SetSkin(Skin skin)
		{
			Skin = skin;
			
			foreach (var skinAvatar in this.skinAvatars)
			{
				skinAvatar.sprite = skin.Sprite;
			}

			var skinState = GetSkinState(skin);
			
			SetState(skinState);
		}

		public void OnClickBuyButton()
		{
			if (Wallet.Coins < Skin.Price)
			{
				return;
			}
		
			Wallet.AddCoins(-Skin.Price);
			SkinChanger.Instance.SetSkin(Skin);
			PlayerSkinService.Instance.AddSkinInProfile(Skin);
			PlayerSkinService.Instance.CurrentSkin = Skin;
			
			SetState(State.Selected);
		}

		public void OnClickSelectButton()
		{
			SkinChanger.Instance.SetSkin(Skin);
			PlayerSkinService.Instance.CurrentSkin = Skin;
			
			SetState(State.Selected);
		}

		public void OnClickWatchAdsButton()
		{
			AdsPanel.Instance.ShowRewardedAd("RewardAds_Skin", WatchAdsRewardAction, AdsPanel.Instance.HidePanel);
		}

		private void WatchAdsRewardAction()
		{
			Skin.CurrentViewCount++;

			SkinRepository.Instance.AddOrUpdateSkinData(Skin);

			if (Skin.CurrentViewCount >= Skin.NeedViewCount)
			{
				PlayerSkinService.Instance.AddSkinInProfile(Skin);
				
				OnClickSelectButton();
			}
			
			AdsPanel.Instance.HidePanel();
		}

		public State GetSkinState(Skin skin)
		{
			var skinState = State.NotAvailable;
		
			if (skin.Available)
			{
				if (PlayerSkinService.Instance.IsSkinInProfile(skin))
				{
					skinState = PlayerSkinService.Instance.CurrentSkin == skin ? State.Selected : State.NotSelected;
				}
			}

			return skinState;
		}

	
		public void SetState(State state)
		{
			switch (state)
			{
				case State.Selected:
					SetSelectedState();
					break;
				case State.NotSelected:
					SetNotSelectedState();
					break;
				case State.CanBuy:
					SetBuyState();
					break;
				case State.Available:
					SetAvailableState();
					break;
				case State.NotAvailable:
					SetNotAvailableState();
					break;
				case State.ForAds:
					SetForAdvState();
					break;
			}
		}
		

		private void SetSelectedState()
		{
			SetAvailableState();
		
			this.selectedImage.gameObject.SetActive(true);

			CurrentState = State.Selected;
		}

		private void SetNotSelectedState()
		{
			SetAvailableState();
		
			this.selectedImage.gameObject.SetActive(false);

			CurrentState = State.NotSelected;
		}
		
		private void SetBuyState()
		{
			SetAvailableState();
		
			this.selectButton.gameObject.SetActive(false);
			this.selectedImage.gameObject.SetActive(false);
			CurrentState = State.CanBuy;
		}
		
		private void SetForAdvState()
		{
			SetAvailableState();
			
			this.selectButton.gameObject.SetActive(false);
			this.selectedImage.gameObject.SetActive(false);
			
			CurrentState = State.ForAds;
		}

		private void SetNotAvailableState()
		{
			this.availablePanel.SetActive(false);
			this.notAvailablePanel.SetActive(true);

			CurrentState = State.NotAvailable;
		}

		private void SetAvailableState()
		{
			this.availablePanel.SetActive(true);
			this.notAvailablePanel.SetActive(false);

			CurrentState = State.Available;
		}
		
		
		private void OnCurrentSkinChanged(Skin skin, Skin prevSkin)
		{
			if (Skin != null && prevSkin != null)
			{
				if (prevSkin == Skin)
				{
					SetState(State.NotSelected);
				}
			}
		}
		
		private void OnSkinAdded(Skin skin)
		{
			if (skin == Skin)
			{
				var state = GetSkinState(Skin);
			
				SetState(state);
			}
		}

		private void OnWalletCoinsCountChanged(int coins)
		{
			var skinState = GetSkinState(Skin);
		
			SetState(skinState);
		}
	}
}