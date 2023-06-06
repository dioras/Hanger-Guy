using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _1.Scripts.GameEvent;
using _1.Scripts.Rope;
using _1.Scripts.Skins;
using Random = UnityEngine.Random;

namespace _1.Scripts.Ui.Shop
{
	public class ShopPanel: MonoBehaviour
	{
		private static readonly int Selected = Animator.StringToHash("Selected");
		private static readonly int Select = Animator.StringToHash("Select");
		private static readonly int Unselect = Animator.StringToHash("Unselect");

		private const string CommonViewCountKey = "CVC";
		private const string RareViewCountKey = "RVC";
		private const string EpicViewCountKey = "EVC";

		public Action<Rarity> OnAdsViewCountChanged { get; set; }

		[SerializeField] private RopeCollision ropeCollision;
		[SerializeField] private Transform champStartPos;
		[SerializeField] private SkinUi skinUiPrefab;
		[SerializeField] private Transform commonTab;
		[SerializeField] private Transform rareTab;
		[SerializeField] private Transform epicTab;
		[SerializeField] private Transform commonTabContent;
		[SerializeField] private Transform rareTabContent;
		[SerializeField] private Transform epicTabContent;
		[SerializeField] private Button commonTabSelectButton;
		[SerializeField] private Button rareTabSelectButton;
		[SerializeField] private Button epicTabSelectButton;
		[SerializeField] private Button[] commonUnlockButtons;
		[SerializeField] private Button[] rareUnlockButtons;
		[SerializeField] private Button[] epicUnlockButtons;
		[SerializeField] private Button commonUnlockCoinButton;
		[SerializeField] private Button rareUnlockCoinButton;
		[SerializeField] private Button epicUnlockCoinButton;
		[SerializeField] private int commonSkinPrice;
		[SerializeField] private int rareSkinPrice;
		[SerializeField] private int epicSkinPrice;
		[SerializeField] private FollowCamera followCamera;
		[SerializeField] private Wallet.Wallet wallet;
		[SerializeField] private Vector3 cameraOffset;
		[SerializeField] private float cameraZoomTime;
		[SerializeField] private int commonNeedViewCount;
		[SerializeField] private int rareNeedViewCount;
		[SerializeField] private int epicNeedViewCount;
		[SerializeField] private TextMeshProUGUI commonViewCountText;
		[SerializeField] private TextMeshProUGUI rareViewCountText;
		[SerializeField] private TextMeshProUGUI epicViewCountText;
		private Skin randomSkinForUnlock;

		public int FirstTimeWatchCommon
		{
			get => PlayerPrefs.GetInt("FTC", 0);
			set
			{
				PlayerPrefs.SetInt("FTC", value);
				PlayerPrefs.Save();
			}
		}

		public int CommonViewCount
		{
			get => PlayerPrefs.GetInt(CommonViewCountKey, 0);
			set
			{
				PlayerPrefs.SetInt(CommonViewCountKey, value);
				PlayerPrefs.Save();
				
				OnAdsViewCountChanged?.Invoke(Rarity.Common);
			}
		}
		
		public int RareViewCount
		{
			get => PlayerPrefs.GetInt(RareViewCountKey, 0);
			set
			{
				PlayerPrefs.SetInt(RareViewCountKey, value);
				PlayerPrefs.Save();
				
				OnAdsViewCountChanged?.Invoke(Rarity.Rare);
			}
		}
		
		public int EpicViewCount
		{
			get => PlayerPrefs.GetInt(EpicViewCountKey, 0);
			set
			{
				PlayerPrefs.SetInt(EpicViewCountKey, value);
				PlayerPrefs.Save();
				
				OnAdsViewCountChanged?.Invoke(Rarity.Epic);
			}
		}




		private void OnEnable()
		{
			UpdateUnlockButtonsState();
			UpdateUnlockForCoinsButtonsState();
			UpdateAdsViewCountText(Rarity.Common);
			UpdateAdsViewCountText(Rarity.Rare);
			UpdateAdsViewCountText(Rarity.Epic);
		}

		private void Start()
		{
			OnAdsViewCountChanged += AdsViewCountChangedHandler;
			PlayerSkinService.Instance.OnSkinAdded += OnSkinAdded;
		
			FillSkinsPanel(SkinRepository.Instance.Skins);
			
			SetCurrentRarityTab(PlayerSkinService.Instance.CurrentSkin.Rarity);
			
			EventRepository.WalletCoinsCountChanged.AddListener(WalletCoinsCountChanged);
		}

		private void OnDestroy()
		{
			OnAdsViewCountChanged-= AdsViewCountChangedHandler;
			PlayerSkinService.Instance.OnSkinAdded -= OnSkinAdded;
			EventRepository.WalletCoinsCountChanged.RemoveListener(WalletCoinsCountChanged);
		}




		private void FillSkinsPanel(Skin[] skins)
		{
			skins = skins.OrderBy(s => s.Price).ToArray();
		
			foreach (var skin in skins)
			{
				if (!skin.Available)
				{
					continue;
				}
				
				var skinUi = Instantiate(this.skinUiPrefab, GetRarityTabContent(skin.Rarity));
				
				skinUi.SetSkin(skin);
			}
		}

		private bool AreAllSkinsUnlocked(Rarity rarity)
		{
			return SkinRepository.Instance.Skins.Count(s =>
				s.Available && s.Rarity == rarity && !PlayerSkinService.Instance.IsSkinInProfile(s)) == 0;
		}
		

		public void OnClickUnlockRandomForAds(int rarityNum)
		{
			var rarity = (Rarity) Enum.Parse(typeof(Rarity), rarityNum.ToString());

			var skins = SkinRepository.Instance.Skins.Where(s =>
				s.Available && s.Rarity == rarity && !PlayerSkinService.Instance.IsSkinInProfile(s)).ToList();

			this.randomSkinForUnlock = skins[Random.Range(0, skins.Count)];

			AdsPanel.Instance.ShowRewardedAd("RewardAds_Skin", UnlockRandomRewardAction, AdsPanel.Instance.HidePanel);
		}

		private void UnlockRandomRewardAction()
		{
			if (this.randomSkinForUnlock != null)
			{
				var giveSkin = false;
			
				if (this.randomSkinForUnlock.Rarity == Rarity.Common)
				{
					CommonViewCount++;

					var needViewCount = FirstTimeWatchCommon == 0 ? 1 : this.commonNeedViewCount;
					
					if (CommonViewCount >= needViewCount)
					{
						giveSkin = true;
						CommonViewCount = 0;
					}

					if (FirstTimeWatchCommon == 0)
					{
						FirstTimeWatchCommon = 1;
					}
				}
				
				if (this.randomSkinForUnlock.Rarity == Rarity.Rare)
				{
					RareViewCount++;

					if (RareViewCount >= this.rareNeedViewCount)
					{
						giveSkin = true;
						RareViewCount = 0;
					}
				}
				
				if (this.randomSkinForUnlock.Rarity == Rarity.Epic)
				{
					EpicViewCount++;

					if (EpicViewCount >= this.epicNeedViewCount)
					{
						giveSkin = true;
						EpicViewCount = 0;
					}
				}

				if (giveSkin)
				{
					PlayerSkinService.Instance.CurrentSkin = this.randomSkinForUnlock;
					PlayerSkinService.Instance.AddSkinInProfile(this.randomSkinForUnlock);
					SkinChanger.Instance.SetSkin(this.randomSkinForUnlock);
				}
			}

			this.randomSkinForUnlock = null;

			AdsPanel.Instance.HidePanel();
		}

		public void OnClickUnlockRandomForCoins(int rarityNum)
		{
			var rarity = (Rarity) Enum.Parse(typeof(Rarity), rarityNum.ToString());
		
			var skins = SkinRepository.Instance.Skins.Where(s =>
				s.Available && s.Rarity == rarity && !PlayerSkinService.Instance.IsSkinInProfile(s)).ToList();
				
			this.randomSkinForUnlock = skins[Random.Range(0, skins.Count)];
			
			var skinPrice = 0;
			
			switch (rarity)
			{
				case Rarity.Common:
					skinPrice = this.commonSkinPrice;
					break;
				case Rarity.Rare:
					skinPrice = this.rareSkinPrice;
					break;
				case Rarity.Epic:
					skinPrice = this.epicSkinPrice;
					break;
			}
		
			if (this.randomSkinForUnlock != null)
			{
				PlayerSkinService.Instance.CurrentSkin = this.randomSkinForUnlock;
				PlayerSkinService.Instance.AddSkinInProfile(this.randomSkinForUnlock);
				SkinChanger.Instance.SetSkin(this.randomSkinForUnlock);
				
			}
		
			switch (rarity)
			{
				case Rarity.Common:
					wallet.AddCoins(-skinPrice);
					break;
				case Rarity.Rare:
					wallet.AddCoins(-skinPrice);
					break;
				case Rarity.Epic:
					wallet.AddCoins(-skinPrice);
					break;
			}
		}

		private void SetCurrentRarityTab(Rarity rarity)
		{
			var rarityTab = GetRarityTab(rarity);
			
			rarityTab.SetAsLastSibling();
			
			var commonAnim = this.commonTabSelectButton.GetComponent<Animator>();
			var rareAnim = this.rareTabSelectButton.GetComponent<Animator>();
			var epicAnim = this.epicTabSelectButton.GetComponent<Animator>();

			switch (rarity)
			{
				case Rarity.Common:
					commonAnim.SetBool(Selected, true);
					commonAnim.SetTrigger(Select);
					rareAnim.SetBool(Selected, false);
					rareAnim.SetTrigger(Unselect);
					epicAnim.SetBool(Selected,false);
					epicAnim.SetTrigger(Unselect);
					
					break;
				case Rarity.Rare:
					rareAnim.SetBool(Selected, true);
					rareAnim.SetTrigger(Select);
					commonAnim.SetBool(Selected, false);
					commonAnim.SetTrigger(Unselect);
					epicAnim.SetBool(Selected,false);
					epicAnim.SetTrigger(Unselect);
					
					break;
				case Rarity.Epic:
					epicAnim.SetBool(Selected, true);
					epicAnim.SetTrigger(Select);
					rareAnim.SetBool(Selected, false);
					rareAnim.SetTrigger(Unselect);
					commonAnim.SetBool(Selected,false);
					commonAnim.SetTrigger(Unselect);
					
					break;
			}
		}
		
		public void SetCurrentRarityTab(int rarityNum)
		{
			var rarity = (Rarity) Enum.Parse(typeof(Rarity), rarityNum.ToString());

			SetCurrentRarityTab(rarity);
		}

		public void ZoomInChamp()
		{
			this.ropeCollision.SetStartJointConnectedAnchor(new Vector3(0f, 0.02f, 0f));
			this.followCamera.Target = this.champStartPos;
			this.followCamera.ChangeOffsetSmoothly(this.cameraOffset, this.cameraZoomTime);
		}

		public void ZoomOutChamp()
		{
			this.ropeCollision.SetStartJointConnectedAnchor(Vector3.zero);
			var champion = FindObjectOfType<Champion.Champion>();
			this.followCamera.Target = champion.transform;
			this.followCamera.ChangeOffsetSmoothly(followCamera.StartOffset, this.cameraZoomTime);
		}
		
		private Transform GetRarityTab(Rarity rarity)
		{
			switch (rarity)
			{
				case Rarity.Common:
					return this.commonTab;
				case Rarity.Rare:
					return this.rareTab;
				case Rarity.Epic:
					return this.epicTab;
				default:
					throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
			}
		}
		
		private Transform GetRarityTabContent(Rarity rarity)
		{
			switch (rarity)
			{
				case Rarity.Common:
					return this.commonTabContent;
				case Rarity.Rare:
					return this.rareTabContent;
				case Rarity.Epic:
					return this.epicTabContent;
				default:
					throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
			}
		}

		private void UpdateUnlockButtonsState()
		{
			if (AreAllSkinsUnlocked(Rarity.Common))
			{
				foreach (var commonUnlockButton in this.commonUnlockButtons)
				{
					commonUnlockButton.gameObject.SetActive(false);
				}
			}
			if (AreAllSkinsUnlocked(Rarity.Rare))
			{
				foreach (var rareUnlockButton in this.rareUnlockButtons)
				{
					rareUnlockButton.gameObject.SetActive(false);
				}
			}
			if (AreAllSkinsUnlocked(Rarity.Epic))
			{
				foreach (var epicUnlockButton in this.epicUnlockButtons)
				{
					epicUnlockButton.gameObject.SetActive(false);
				}
			}
		}

		private void UpdateUnlockForCoinsButtonsState()
		{
			this.commonUnlockCoinButton.interactable = wallet.Coins >= this.commonSkinPrice;
			this.rareUnlockCoinButton.interactable = wallet.Coins >= this.rareSkinPrice;
			this.epicUnlockCoinButton.interactable = wallet.Coins >= this.epicSkinPrice;
		}

		private void UpdateAdsViewCountText(Rarity rarity)
		{
			if (rarity == Rarity.Common)
			{
				var needViewCount = FirstTimeWatchCommon == 0 ? 1 : this.commonNeedViewCount;
				
				this.commonViewCountText.text = $"{CommonViewCount}/{needViewCount}";
			}
			if (rarity == Rarity.Rare)
			{
				this.rareViewCountText.text = $"{RareViewCount}/{this.rareNeedViewCount}";
			}
			if (rarity == Rarity.Epic)
			{
				this.epicViewCountText.text = $"{EpicViewCount}/{this.epicNeedViewCount}";
			}
		}

		private void AdsViewCountChangedHandler(Rarity rarity)
		{
			UpdateAdsViewCountText(rarity);
		}
		
		private void OnSkinAdded(Skin skin)
		{
			UpdateUnlockButtonsState();
			UpdateAdsViewCountText(skin.Rarity);
		}
		
		private void WalletCoinsCountChanged(int coins)
		{
			UpdateUnlockForCoinsButtonsState();
		}
	}
}