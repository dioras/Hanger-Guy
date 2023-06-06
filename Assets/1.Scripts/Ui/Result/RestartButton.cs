using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _1.Scripts.Ui.Result
{
    public class RestartButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(RestartLevel);
        }

        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveListener(RestartLevel);
        }
        
        

        private void RestartLevel()
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