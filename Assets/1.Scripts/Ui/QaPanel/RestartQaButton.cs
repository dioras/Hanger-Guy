using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _1.Scripts.Ui.QaPanel
{
    public class RestartQaButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(RestartButton);
        }

        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveListener(RestartButton);
        }

        
        
        private void RestartButton()
        {
            var splashScreen = FindObjectOfType<SplashScreenUI>(true);
            
            splashScreen.gameObject.SetActive(true);
            
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            var sceneAsync = SceneManager.LoadSceneAsync(0);

            while (!sceneAsync.isDone)
            {
                yield return null;
            }
        }
    }
}