using UnityEngine;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Rope
{
    public class RopeInitor : MonoBehaviour
    {
        private void Awake()
        {
            EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        }

        
        
        private void OnGameStateChanged(GameStateEnum gameStateEnum)
        {
            if (gameStateEnum == GameStateEnum.Play)
            {
                GetComponent<RopeLength>().enabled = true;
            }
            else if (gameStateEnum == GameStateEnum.Result)
            {
                GetComponent<RopeLength>().enabled = false;
                
                var robCollision = GetComponent<RopeCollision>();
                robCollision.Disconnect();
                robCollision.enabled = false;
            }
            else if (gameStateEnum == GameStateEnum.Lose)
            {
                var ropeLength = GetComponent<RopeLength>();
                ropeLength.ResetLength();
                ropeLength.enabled = false;
                
                var robCollision = GetComponent<RopeCollision>();
                robCollision.Disconnect();
            }
        }
    }
}