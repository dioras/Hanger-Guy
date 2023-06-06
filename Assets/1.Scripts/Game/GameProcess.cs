using System.Collections;
using System.Collections.Generic;
using _1.Scripts.GameEvent;
using _1.Scripts.Ui;
using UnityEngine;

namespace _1.Scripts.Game
{
    public class GameProcess : MonoBehaviour
    {
        public GameStateEnum GameState { get; private set; }
        
        [SerializeField] private List<GamePanel> gamePanels;
        [SerializeField] 
        private float losePanelShowDelay;
        
        
        
        private void Awake()
        {
#if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 45;
#else
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 0;
#endif
        
            ApplyGameState(GameStateEnum.Init);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Break();
            }
        }


        public void ApplyGameState(GameStateEnum gameState)
        {
            if (GameState == gameState || 
                (gameState == GameStateEnum.Play && (GameState == GameStateEnum.Result || GameState == GameStateEnum.Lose)))
            {
                return;
            }

            GameState = gameState;

            foreach (var gamePanel in this.gamePanels)
            {
                if (gamePanel.ActivateState == GameStateEnum.None)
                {
                    continue;
                }

                var active = (gamePanel.ActivateState & gameState) != GameStateEnum.None;

                if (gameState == GameStateEnum.Lose)
                {
                    StartCoroutine(ActivateWithDelay(gamePanel.gameObject, active, this.losePanelShowDelay));
                }
                else
                {
                    gamePanel.gameObject.SetActive(active);
                }
            }
            
            EventRepository.GameStateChanged.Invoke(GameState);
        }

        private IEnumerator ActivateWithDelay(GameObject gameObject, bool active, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            gameObject.SetActive(active);
        }
    }
}