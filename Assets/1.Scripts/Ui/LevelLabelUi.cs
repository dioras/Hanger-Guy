using System.Collections;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;
using TMPro;
using UnityEngine;

namespace _1.Scripts.Ui
{
    public class LevelLabelUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelTmp;
        [SerializeField] private float hideDelay = 2f;
        
        
        
        private void Awake()
        {
            SetLevel();
            EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        }

        
        
        private void OnGameStateChanged(GameStateEnum gameState)
        {
            if (gameState != GameStateEnum.Play)
            {
                return;
            }

            StartCoroutine(HideWithDelay());
        }

        private void SetLevel()
        {
            this.levelTmp.text = $"Level {PlayerPrefs.GetInt("current_level") + 1}";
        }

        private IEnumerator HideWithDelay()
        {
            var originalColor = this.levelTmp.color;
            var targetColor = this.levelTmp.color;
            targetColor.a = 0f;
            
            for (var i = 0f; i < 1; i += Time.deltaTime / this.hideDelay)
            {
                this.levelTmp.color = Color.Lerp(originalColor, targetColor, i);
                yield return null;
            }

            this.levelTmp.color = targetColor;
        }
    }
}