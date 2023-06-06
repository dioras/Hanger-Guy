using _1.Scripts.Game;
using _1.Scripts.GameEvent;
using UnityEngine;

namespace _1.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        private void Awake()
        {
            EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        }

        
        
        private void OnGameStateChanged(GameStateEnum gameState)
        {
            if (gameState == GameStateEnum.Lose || gameState == GameStateEnum.Result)
            {
                GetComponent<FollowCamera>().enabled = false;
            }
            else if (gameState == GameStateEnum.Init || gameState == GameStateEnum.Continue)
            {
                GetComponent<FollowCamera>().enabled = true;
            }
        }
    }
}