using UnityEngine;

namespace _1.Scripts.Skins
{
	public class Skin: ScriptableObject
	{
		[field: SerializeField] public int Id { get; set; }
		[field: SerializeField] public string Name { get; set; }
		[field: SerializeField] public Sprite Sprite { get; set; }
		[field: SerializeField] public bool Available { get; set; }
		[field: SerializeField] public bool ForAds { get; set; }
		[field: SerializeField] public int NeedViewCount { get; set; }
		public int CurrentViewCount { get; set; }
		[field: SerializeField] public int Price { get; set; }
		[field: SerializeField] public Rarity Rarity { get; set; }
		
		
		public void UpdateData(SkinData skinData)
		{
			if (!skinData.Name.Equals(Name))
			{	
				return;
			}

			Available = skinData.Available;
			CurrentViewCount = skinData.AdsViewCount;
		}
	}
}