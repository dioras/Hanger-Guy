using System.Linq;
using UnityEngine;
using _1.Scripts.Rounds;

namespace _1.Scripts.Level
{
	public class ColorSchemeChanger: MonoBehaviour, IColorSchemeTaker
	{
		public const string CurrSchemeKey = "CurrCS";

		public ColorScheme CurrColorScheme
		{
			get
			{
				return ColorSchemes.First(s => s.name.Equals(PlayerPrefs.GetString(CurrSchemeKey, "Scheme1")));
			}
			set
			{
				if (value.name.Equals(CurrColorScheme.name))
				{
					return;
				}
			
				PlayerPrefs.SetString(CurrSchemeKey, value.name);
				PlayerPrefs.Save();
			}
		}
	
		[field: SerializeField] public ColorScheme[] ColorSchemes { get; set; }
		[field: SerializeField] public GameObject[] Backgrounds { get; set; }
		[field: SerializeField] public int Periodicity { get; set; }
		
		

		private void Start()
		{
			var currLvlIdx = FindObjectOfType<RoundRepository>().CurrentLevelIndex;
		
			if (currLvlIdx % Periodicity == 0)
			{
				CurrColorScheme = ColorSchemes[currLvlIdx/Periodicity % ColorSchemes.Length];
			}
			
			ApplyColorScheme(CurrColorScheme);
			var colorSchemeIdx = ColorSchemes.ToList().IndexOf(CurrColorScheme);
			
			ShowBackground(colorSchemeIdx);
		}




		public void ApplyColorScheme(ColorScheme colorScheme)
		{
			var colorSchemeTakers = FindObjectsOfType<ColorSchemeTaker>();

			foreach (var colorSchemeTaker in colorSchemeTakers)
			{
				colorSchemeTaker.ApplyColorScheme(colorScheme);
			}
		}

		public void ShowBackground(int idx)
		{
			for (var i = 0; i < Backgrounds.Length; i++)
			{
				Backgrounds[i].SetActive(i == idx);
			}
		}
	}
}