using _1.Scripts.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _1.Scripts.Ui.InitPanel
{
    public class TapToPlay : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            FindObjectOfType<GameProcess>().ApplyGameState(GameStateEnum.Play);
        }
    }
}