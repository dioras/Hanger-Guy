using UnityEngine;

namespace _1.Scripts.Level
{
	[CreateAssetMenu(fileName = "New color scheme", menuName = "ColorScheme")]
	public class ColorScheme: ScriptableObject
	{
		[field: SerializeField] public Material FrontPanelBoxMat { get; set; }
		[field: SerializeField] public Material MainBoxMat { get; set; }
	}
}