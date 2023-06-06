using System;
using System.Linq;
using UnityEngine;

namespace _1.Scripts.Skins
{
	public class PlayerSkinService: MonoBehaviour, ISkinService
	{
		public static PlayerSkinService Instance { get; private set; }
	
		private const string CurrentSkinKey = "CurrSkin";
		private const string SkinsKey = "Skins";

		public Action<Skin, Skin> OnCurrentSkinChanged { get; set; }
		public Action<Skin> OnSkinAdded { get; set; }
		
		
		public Skin CurrentSkin
		{
			get => SkinRepository.Instance.Skins.Single(s => s.Name.Equals(PlayerPrefs.GetString(CurrentSkinKey, 
			SkinRepository.Instance.DefaultSkins[0].Name)));
			set
			{
				if (CurrentSkin == value)
				{
					return;
				}

				var prevSkin = CurrentSkin;
			
				PlayerPrefs.SetString(CurrentSkinKey, value.Name);
				PlayerPrefs.Save();
				
				OnCurrentSkinChanged?.Invoke(value, prevSkin);
			}
		}



		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				if (Instance != this)
				{
					Destroy(this.gameObject);
				}

				return;
			}

			DontDestroyOnLoad(this.gameObject);
		
			foreach (var skin in SkinRepository.Instance.DefaultSkins)
			{
				if (!IsSkinInProfile(skin))
				{
					AddSkinInProfile(skin);
				}
			}
		}



		public int GetSkinsInProfileAmount()
		{
			var SkinsInProfileString = GetSkinsInProfileString();

			return SkinsInProfileString.Split(',').Length;
		}
		
		public bool IsSkinInProfile(string skin)
		{
			if (string.IsNullOrEmpty(skin))
			{
				return false;
			}

			var skins = GetSkinsInProfileString().Split(',');

			foreach (var s in skins)
			{
				if (string.IsNullOrWhiteSpace(s))
				{
					continue;
				}

				if (s.Equals(skin))
				{
					return true;
				}
			}
			
			return false;
		} 
		
		public bool IsSkinInProfile(Skin skin)
		{
			return IsSkinInProfile(skin.Name);
		} 
		
		public void BuySkin(string skin)
		{
			AddSkinInProfile(skin);
		}

		public void RemoveSkin(string skin)
		{
			var SkinsInProfileString = GetSkinsInProfileString();
			SkinsInProfileString = SkinsInProfileString
				.Replace(skin, string.Empty)
				.Replace(",,", string.Empty)
				.Trim(',');
			
			AddSkinInProfile(SkinsInProfileString);
		}
		
		public string[] GetBoughtSkins()
		{
			var SkinsInProfileString = GetSkinsInProfileString();
			return SkinsInProfileString.Split(',');
		}
		
		public void AddSkinInProfile(string skin)
		{
			if (IsSkinInProfile(skin))
			{
				return;
			}
			
			var SkinsInProfileString = GetSkinsInProfileString();
			SkinsInProfileString += $"{skin},";
			
			PlayerPrefs.SetString(SkinsKey, SkinsInProfileString);
			PlayerPrefs.Save();
			
			OnSkinAdded?.Invoke(SkinRepository.Instance.Skins.Single(s => s.Name.Equals(skin)));
		}
		
		public void AddSkinInProfile(Skin skin)
		{
			if (IsSkinInProfile(skin.Name))
			{
				return;
			}
			
			AddSkinInProfile(skin.Name);
		}
		
		public string GetSkinsInProfileString()
		{
			var SkinsInProfileString = PlayerPrefs.GetString(SkinsKey, SkinsToString(SkinRepository.Instance.DefaultSkins));
			
			return SkinsInProfileString;
		}

		private string SkinsToString(Skin[] skins)
		{
			return skins.Aggregate(string.Empty, (current, skin) => current + skin.Name + ",");
		}
	}
}