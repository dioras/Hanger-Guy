using System;
using System.Linq;
using UnityEngine;

namespace _1.Scripts.Skins
{
	public class SkinRepository: MonoBehaviour
	{
		public static SkinRepository Instance { get; private set; }

		public const string SkinsDataKey = "skinsdata";

		[field: SerializeField]
		public Skin[] DefaultSkins { get; set;}

		[field: SerializeField]
		public CharacterSkin[] CharacterSkins { get; set; }

		public Skin[] Skins { get; set; }

		private SkinsData skinsData;
		
		
		
		
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

			Skins = CharacterSkins.ToArray<Skin>();
			
			this.skinsData = LoadSkinData();

			if (this.skinsData.Skins != null)
			{
				foreach (var skin in CharacterSkins)
				{
					var skinData = this.skinsData.Skins.SingleOrDefault(s => s.Name.Equals(skin.Name));

					if (skinData == null)
					{
						continue;
					}

					skin.UpdateData(skinData);
				}
			}
		}
		
		
		private void OnApplicationQuit()
		{
			SaveSkinData();
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (!hasFocus)
			{
				SaveSkinData();
			}
		}
		
		public void SaveSkinData()
		{
			var json = JsonUtility.ToJson(this.skinsData);
		
			PlayerPrefs.SetString(SkinsDataKey, json);
			PlayerPrefs.Save();
		}
		
		private SkinsData LoadSkinData()
		{
			SkinsData data;
		
			if (PlayerPrefs.HasKey(SkinsDataKey))
			{
				data = JsonUtility.FromJson<SkinsData>(PlayerPrefs.GetString(SkinsDataKey));
			}
			else
			{
				data = new SkinsData();
			}
			
			return data;
		}


		public void AddOrUpdateSkinData(Skin skin)
		{
			var skinData = this.skinsData.Skins.SingleOrDefault(s => s.Name.Equals(skin.Name));
		
			if (skinData == null)
			{
				skinData = new SkinData
				{
					Name = skin.Name
				};

				this.skinsData.Skins.Add(skinData);
			}
			
			skinData.Available = skin.Available;
			skinData.AdsViewCount = skin.CurrentViewCount;
		}
	}
}