using UnityEngine;
using UnityEngine.UI;

namespace _1.Scripts.Ui.QaPanel
{
    public class StopChampionButton : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(StopChar);
        }

        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveListener(StopChar);
        }

        
        
        private void StopChar()
        {
            this.rb.useGravity = !this.rb.useGravity;
            this.rb.isKinematic = !this.rb.isKinematic;
        }
    }
}