using System.Collections.Generic;
using System.Diagnostics;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;
using _1.Scripts.Champion;

namespace _1.Scripts.Rounds
{
    public class RoundRepository : MonoBehaviour
    {
        public int CurrLvl
        {
            get => PlayerPrefs.GetInt("CurrLvl", 1);
            set
            {
                PlayerPrefs.SetInt("CurrLvl", value);
                PlayerPrefs.Save();
            }
        }
        public int CurrentLevelIndex => this._currentLevel;
        public string CurrentLevelName => this._currentLevelName;
        public int LevelsCount => this.levels.Count;
        
        [SerializeField] private List<GameObject> levels;
        [SerializeField] private int minRangeLevel = 8;

        private int _currentLevel;
        private string _currentLevelName;
        
        private Stopwatch stopwatch;



        public void LoadLevel()
        {
            SceneManager.LoadSceneAsync(0);
        }

        

        private void Awake()
        {
            EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnEnable()
        {
            this._currentLevel = PlayerPrefs.GetInt("current_level", 0);
            
            this._currentLevelName =
                this.levels[
                    this._currentLevel < this.levels.Count
                        ? this._currentLevel
                        : PlayerPrefs.GetInt("current_level_index")].name;

            if (this._currentLevel < this.levels.Count) // 0..5 ? 6
            {
                this.levels[this._currentLevel].SetActive(true);
            }
            else
            {
                this.levels[PlayerPrefs.GetInt("current_level_index")].SetActive(true);
            }
        }

        private void OnDestroy()
        {
            EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        }
        


        private void OnGameStateChanged(GameStateEnum gameState)
        {
            switch (gameState)
            {
                case GameStateEnum.Play:
                    this.stopwatch = Stopwatch.StartNew();
                    break;
                
                case GameStateEnum.Result:
                    this.stopwatch?.Stop();
                    var time = this.stopwatch != null ? (int)this.stopwatch.Elapsed.TotalSeconds : 0;

                    var matchCoinCollector = FindObjectOfType<MatchCoinCollector>();
                    var killCounter = FindObjectOfType<KillCounter>();
                    
                    
                    break;
                
                case GameStateEnum.Lose:
                    this.stopwatch?.Stop();
                    time = this.stopwatch != null ? (int)this.stopwatch.Elapsed.TotalSeconds : 0;
                    
                    matchCoinCollector = FindObjectOfType<MatchCoinCollector>();
                    killCounter = FindObjectOfType<KillCounter>();
                    
                    
                    break;
            }
        
            if (gameState == GameStateEnum.Result)
            {
                ++this._currentLevel;

                CurrLvl++;

                PlayerPrefs.SetInt("current_level", this._currentLevel);
                PlayerPrefs.Save();

                if (this._currentLevel >= this.levels.Count) // 0..5 ? 6
                {
                    var curLevelIndex = PlayerPrefs.GetInt("current_level_index", -1);
                    int range;

                    do
                    {
                        range = Random.Range(minRangeLevel, this.levels.Count);
                        PlayerPrefs.SetInt("current_level_index", range);
                        PlayerPrefs.Save();
                    } while (curLevelIndex == range);
                }
            }
        }
    }
}