using UnityEngine;
using UnityEngine.UI;

namespace _1.Scripts.Ui
{
	public class MultiImageButton : Button
	{
		public Graphic[] Graphics;


		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			Color color;
			switch (state)
			{
				case (SelectionState.Normal):
					color = this.colors.normalColor;
					break;

				case (SelectionState.Highlighted):
					color = this.colors.highlightedColor;
					break;

				case (SelectionState.Pressed):
					color = this.colors.pressedColor;
					break;

				case (SelectionState.Disabled):
					color = this.colors.disabledColor;
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

					default:
						throw new System.NotSupportedException();
				}
			}
		}


		private void ColorTween(Color targetColor, bool instant)
		{
			if (this.targetGraphic == null)
			{
				return;
			}

			foreach (var g in this.Graphics)
			{
				g.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);
			}
		}
	}
}