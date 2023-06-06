using UnityEngine.Events;
using _1.Scripts.GameEvent.Events;

namespace _1.Scripts.GameEvent
{
    public static class EventRepository
    {
        public static readonly GameStateChangedEvent GameStateChanged; 
        public static readonly ChampionHealthChangedEvent ChampionHealthChanged; 
        public static readonly CoinPickedUpEvent CoinPickedUp; 
        public static readonly WalletCoinsCountChangedEvent WalletCoinsCountChanged; 
        public static readonly UnityEvent OnCollisionWithCharacter; 
        
        
        
        static EventRepository()
        {
            GameStateChanged = new GameStateChangedEvent();
            ChampionHealthChanged = new ChampionHealthChangedEvent();
            CoinPickedUp = new CoinPickedUpEvent();
            WalletCoinsCountChanged = new WalletCoinsCountChangedEvent();
            OnCollisionWithCharacter = new UnityEvent();
        }
    }
}