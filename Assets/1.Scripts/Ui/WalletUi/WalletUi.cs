using System;
using _1.Scripts.GameEvent;
using TMPro;
using UnityEngine;

namespace _1.Scripts.Ui.WalletUi
{
    public class WalletUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsCounter;
        private Wallet.Wallet Wallet
        {
            get
            {
                if (this.wallet == null)
                {
                    this.wallet = FindObjectOfType<Wallet.Wallet>();
                }

                return this.wallet;
            }
        }

        private Wallet.Wallet wallet;
        
        

        
        private void Awake()
        {
            EventRepository.WalletCoinsCountChanged.AddListener(OnCoinsCountChanged);
        }
        
        private void Update()
        {
            if (Wallet != null)
            {
                this.coinsCounter.text = Wallet.Coins.ToString();
            }
        }

        private void OnDestroy()
        {
            EventRepository.WalletCoinsCountChanged.RemoveListener(OnCoinsCountChanged);
        }

        
        
        private void OnCoinsCountChanged(int count)
        {
            this.coinsCounter.text = count.ToString();
        }

        public void OnClickCoin()
        {
            if (Debug.isDebugBuild)
            {
                Wallet.AddCoins(100);
            }
        }
    }
}