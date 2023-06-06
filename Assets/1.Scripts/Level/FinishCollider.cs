using _1.Scripts.Champion;
using _1.Scripts.Game;
using UnityEngine;

namespace _1.Scripts.Level
{
    public class FinishCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ChampionHealth>(out var championHealth) && championHealth.Health > 0)
            {
                FindObjectOfType<GameProcess>().ApplyGameState(GameStateEnum.Result);
            }
        }
    }
}