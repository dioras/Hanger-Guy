using TMPro;
using UnityEngine;
using _1.Scripts.Game;

namespace _1.Scripts.Ui
{
	public class MatchCoinsUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI coinsCounter;

		private MatchCoinCollector MatchCoinCollector
		{
			get
			{
				if (this.matchCoinCollector == null)
				{
					this.matchCoinCollector = FindObjectOfType<MatchCoinCollector>();
				}

				return this.matchCoinCollector;
			}
		}

		private MatchCoinCollector matchCoinCollector;



		private void Update()
		{
			if (MatchCoinCollector != null)
			{
				this.coinsCounter.text = MatchCoinCollector.Instance.CoinsAmount.ToString();
			}
		}
	}
}