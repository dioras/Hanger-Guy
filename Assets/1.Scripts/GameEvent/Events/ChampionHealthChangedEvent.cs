using UnityEngine.Events;
using _1.Scripts.Champion;

namespace _1.Scripts.GameEvent.Events
{
    public class ChampionHealthChangedEvent : UnityEvent<ChampionHealth, int, int>
    {
        
    }
}