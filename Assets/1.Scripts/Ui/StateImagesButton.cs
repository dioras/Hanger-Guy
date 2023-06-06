using UnityEngine;
using UnityEngine.UI;

namespace _1.Scripts.Ui
{
	public class StateImagesButton: Button
	{
		[SerializeField] private Image[] images;
		[SerializeField] private Sprite[] normalSprites;
		[SerializeField] private Sprite[] disabledSprites;
	
		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			Color color;
		
			switch (state)
			{
				case (SelectionState.Normal):
					color = this.colors.normalColor;
				
					for (var i = 0; i < this.images.Length; i++)
					{
						this.images[i].sprite = this.normalSprites[i];
					}
					
					break;

				case (SelectionState.Highlighted):
					color = this.colors.highlightedColor;
					break;

				case (SelectionState.Pressed):
					color = this.colors.pressedColor;
					break;

				case (SelectionState.Disabled):
					color = this.colors.normalColor;
					for (var i = 0; i < this.images.Length; i++)
					{
						this.images[i].sprite = this.disabledSprites[i];
					}
					break;

				default:
					color = this.colors.normalColor;
				
					break;
			}
			
			if (gameObject.activeInHierarchy)
			{
				switch (this.transition)
				{
					case (Transition.ColorTint):
						this.ColorTween(color * this.colors.colorMultiplier, instant);
						break;
				}
			}
		}
		private void ColorTween(Color targetColor, bool instant)
		{
			if (this.targetGraphic == null)
			{
				return;
			}

			foreach (var g in this.images)
			{
				g.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);
			}
		}
	}
}