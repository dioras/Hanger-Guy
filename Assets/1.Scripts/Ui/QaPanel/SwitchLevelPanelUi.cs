using _1.Scripts.Rounds;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _1.Scripts.Ui.QaPanel
{
    public class SwitchLevelPanelUi : MonoBehaviour
    {
        [SerializeField] private Button goButton;
        [SerializeField] private InputField levelInputField;
        
        
        
        private void Awake()
        {
            goButton.onClick.AddListener(OnGoButtonClicked);
        }

        private void OnDestroy()
        {
            goButton.onClick.RemoveListener(OnGoButtonClicked);
        }

        
        
        private void OnGoButtonClicked()
        {
            if (string.IsNullOrWhiteSpace(levelInputField.text))
            {
                return;
            }

            if (!int.TryParse(levelInputField.text, out var levelIndex))
            {
                return;
            }

            if (levelIndex <= 0)
            {
                return;
            }

            var roundRepository = FindObjectOfType<RoundRepository>();
            if (levelIndex > roundRepository.LevelsCount)
            {
                return;
            }
            
            PlayerPrefs.SetInt("current_level", levelIndex - 1);
            PlayerPrefs.SetInt("current_level_index", levelIndex - 1);
            PlayerPrefs.Save();
            
            roundRepository.LoadLevel();
        }
    }
}