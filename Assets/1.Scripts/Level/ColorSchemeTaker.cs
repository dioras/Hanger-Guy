using UnityEngine;

namespace _1.Scripts.Level
{
	public class ColorSchemeTaker: MonoBehaviour, IColorSchemeTaker
	{
		[SerializeField] private Renderer[] frontPanelBoxRenderers;
		[SerializeField] private Renderer[] mainBoxRenderers;



		public void ApplyColorScheme(ColorScheme scheme)
		{
			foreach (var frontPanelBoxRenderer in this.frontPanelBoxRenderers)
			{
				frontPanelBoxRenderer.material = scheme.FrontPanelBoxMat;
			}

			foreach (var mainBoxRenderer in this.mainBoxRenderers)
			{
				mainBoxRenderer.material = scheme.MainBoxMat;
			}
		}
	}
}