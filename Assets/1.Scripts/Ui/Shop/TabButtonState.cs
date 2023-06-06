using UnityEngine;
using UnityEngine.UI;
using _1.Scripts.Skins;

namespace _1.Scripts.Ui.Shop
{
	public class TabButtonState: MonoBehaviour
	{
		private static readonly int Selected = Animator.StringToHash("Selected");
	
		[SerializeField] private Button commonTabSelectButton;
		[SerializeField] private Button rareTabSelectButton;
		[SerializeField] private Button epicTabSelectButton;




		private void Start()
		{
			UpdateTabButtonsState(PlayerSkinService.Instance.CurrentSkin.Rarity);
		}


		private void UpdateTabButtonsState(Rarity rarity)
		{
			var commonAnim = this.commonTabSelectButton.GetComponent<Animator>();
			var rareAnim = this.rareTabSelectButton.GetComponent<Animator>();
			var epicAnim = this.epicTabSelectButton.GetComponent<Animator>();

			switch (rarity)
			{
				case Rarity.Common:
					commonAnim.SetBool(Selected, true);
					rareAnim.SetBool(Selected, false);
					epicAnim.SetBool(Selected, false);
					break;
				case Rarity.Rare:
					commonAnim.SetBool(Selected, false);
					rareAnim.SetBool(Selected, true);
					epicAnim.SetBool(Selected, false);
					break;
				case Rarity.Epic:
					commonAnim.SetBool(Selected, false);
					rareAnim.SetBool(Selected, false);
					epicAnim.SetBool(Selected, true);
					break;
			}
		}
	}
}