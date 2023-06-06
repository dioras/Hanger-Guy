using TMPro;
using UnityEngine;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;

namespace _1.Scripts.Ui.QaPanel
{
    public class ChampionSpeed : MonoBehaviour
    {
        private Rigidbody rb;
        
        private TextMeshProUGUI _tmp;


        
        private void Awake()
        {
            this._tmp = GetComponent<TextMeshProUGUI>();
            
            EventRepository.GameStateChanged.AddListener(GameStateChanged);
        }

        private void Update()
        {
            if (ReferenceEquals(this.rb, null))
            {
                return;
            }
        
            this._tmp.text = $"Speed: {this.rb.velocity.magnitude:00.00}";
        }

        private void OnDestroy()
        {
            EventRepository.GameStateChanged.RemoveListener(GameStateChanged);
        }




        private void GameStateChanged(GameStateEnum gameState)
        {
            if (gameState == GameStateEnum.Play)
            {
                rb = FindObjectOfType<Champion.Champion>().GetComponent<Rigidbody>();
            }
        }
    }
}