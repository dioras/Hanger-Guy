using _1.Scripts.Champion;
using TMPro;
using UnityEngine;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Ui.QaPanel
{
    public class ChampionHealthUi : MonoBehaviour
    {
        private ChampionHealth health;
        
        private TextMeshProUGUI _tmp;



        
        private void Awake()
        {
            this._tmp = GetComponent<TextMeshProUGUI>();
            
            EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        }

        private void Update()
        {
            if (ReferenceEquals(this.health, null))
            {
                return;
            }
            
            this._tmp.text = $"Health: {this.health.Health:000}";
        }
        
        
        
        private void OnGameStateChanged(GameStateEnum gameState)
        {
            if (gameState == GameStateEnum.Play)
            {
                this.health = FindObjectOfType<ChampionHealth>();
            }
        }
    }
}