using System;
using _1.Scripts.GameEvent;
using UnityEngine;

namespace _1.Scripts.Wallet
{
    public class Wallet : MonoBehaviour
    {
        public Action<int> OnCoinsChanged { get; set; }
        public int Coins { get; private set; }

        private const string CoinsCountKey = "coins_count";
        
        
        
        private void Awake()
        {
            Init();
        }

        public void AddCoins(int add)
        {
            this.Coins += add;
            SaveCoinsCount();
            EventRepository.WalletCoinsCountChanged.Invoke(this.Coins);
        }

        private void Init()
        {
            this.Coins = PlayerPrefs.GetInt(CoinsCountKey, 0);
            EventRepository.WalletCoinsCountChanged.Invoke(this.Coins);
        }

        private void SaveCoinsCount()
        {
            PlayerPrefs.SetInt(CoinsCountKey, Coins);
            PlayerPrefs.Save();
        }
    }
}