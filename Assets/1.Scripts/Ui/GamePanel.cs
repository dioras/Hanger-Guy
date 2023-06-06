using _1.Scripts.Game;
using UnityEngine;

namespace _1.Scripts.Ui
{
    public class GamePanel : MonoBehaviour
    {
        public GameStateEnum ActivateState => this.activateState; 
        
        [SerializeField] private GameStateEnum activateState;
    }
}