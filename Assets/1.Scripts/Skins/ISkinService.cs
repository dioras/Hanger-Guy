using System;

namespace _1.Scripts.Skins
{
	public interface ISkinService
	{
		Action<Skin, Skin> OnCurrentSkinChanged { get; set; }
		Action<Skin> OnSkinAdded { get; set; }
	
		Skin CurrentSkin { get; set; }
		int GetSkinsInProfileAmount();
		bool IsSkinInProfile(string skin);
		bool IsSkinInProfile(Skin skin);
		void BuySkin(string skin);
		void RemoveSkin(string skin);
		string[] GetBoughtSkins();
		void AddSkinInProfile(string skin);
		void AddSkinInProfile(Skin skin);
		string GetSkinsInProfileString();
	}
}