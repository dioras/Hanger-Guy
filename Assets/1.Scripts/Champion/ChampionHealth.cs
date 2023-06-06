using System;
using System.Collections;
using System.Collections.Generic;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;
using _1.Scripts.Level;
using _1.Scripts.Settings.Champion;
using UnityEngine;

namespace _1.Scripts.Champion
{
    [Serializable]
    public class ChampionParts
    {
        [field:SerializeField]
        public List<ChampionPart> Parts { get; set; }
    }

    public class ChampionHealth : MonoBehaviour
    {
        public int Health { get; private set; }

        public Rigidbody[] Rigidbodies { get; private set; }
        public int ChampionPartsNum => this.championParts.Count;
        public List<ChampionParts> ChampionParts => this.championParts;
        public bool IsInvulnerable { get; set; }

        [SerializeField] private List<ChampionParts> championParts;
        [SerializeField] private float minSpeedForDamage;
        [SerializeField] private float damageDelay = .3f;
        [SerializeField] private int partsPerSaw = 3;
        [SerializeField] private ChampionAdditionHealth championAdditionHealth;

        private new Rigidbody rigidbody;
        private float _currentDelay;
    
    


        private void Awake()
        {
            Rigidbodies = GetComponentsInChildren<Rigidbody>();

            int addHealth;
        
            switch (PlayerPrefs.GetInt("current_level"))
            {
                case 0:
                    addHealth = this.championAdditionHealth.Level1;
                    break;
                
                case 1: 
                    addHealth = this.championAdditionHealth.Level2;
                    break;
                
                case 2: 
                    addHealth = this.championAdditionHealth.Level3;
                    break;
                
                case 3: 
                    addHealth = this.championAdditionHealth.Level4;
                    break;
                
                default:
                    addHealth = this.championAdditionHealth.Other;
                    break;
            }
            
            this.rigidbody = GetComponent<Rigidbody>();
            Health = this.championParts.Count + addHealth;
            
            EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void Update()
        {
            if (this._currentDelay > 0)
            {
                this._currentDelay -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!this.enabled)
            {
                return;
            }
        
            if (!other.gameObject.TryGetComponent<LevelDamage>(out _))
            {
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Saw") && this._currentDelay <= 0f)
            {
                this._currentDelay = this.damageDelay;
                if (!IsInvulnerable)
                {
                    TakeDamage(this.partsPerSaw);
                }

                return;
            }

            if (this.rigidbody.velocity.magnitude >= this.minSpeedForDamage && this._currentDelay <= 0f)
            {
                this._currentDelay = this.damageDelay;
                
                var partsNum = (int) (this.rigidbody.velocity.magnitude / this.minSpeedForDamage);
                if (!IsInvulnerable)
                {
                    TakeDamage(partsNum);
                }
            }
        }

        private void OnDestroy()
        {
            EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        }




        public void DetachParts(int num)
        {
            if (Health - num <= ChampionPartsNum)
            {
                var startPartIdx = Health - 1;
                
                if (Health > ChampionPartsNum)
                {
                    startPartIdx = ChampionPartsNum - 1;
                }
                
                var endPartIdx = Health - 1 - num;

                if (endPartIdx < 0)
                {
                    endPartIdx = -1;
                }

                for (var i = startPartIdx; i > endPartIdx; i--)
                {
                    foreach (var championPart in this.championParts[i].Parts)
                    {
                        championPart.Detach();
                    }
                }
            }
        }

        public void RestoreParts(int num)
        {
            if (Health == ChampionPartsNum)
            {
                return;
            }

            var prevHealth = Health;
        
            Health = num;
            
            EventRepository.ChampionHealthChanged.Invoke(this, Health, prevHealth);

            var restorePartsNum = num;

            if (restorePartsNum > ChampionPartsNum)
            {
                restorePartsNum = ChampionPartsNum;
            }

            for (var i = 0; i < restorePartsNum; i++)
            {
                foreach (var championParts in this.championParts[i].Parts)
                {
                    championParts.Attach();
                }
            }
        }

        public void AddHealth(int health)
        {
            if (Health == ChampionPartsNum)
            {
                return;
            }
        
            var newHealth = Health + health;
        
            if (newHealth > ChampionPartsNum)
            {
                newHealth = ChampionPartsNum;
            }

            var prevHealth = Health;

            Health = newHealth;
            
            EventRepository.ChampionHealthChanged.Invoke(this, Health, prevHealth);
            
            for (var i = prevHealth; i < Health; i++)
            {
                foreach (var championParts in this.championParts[i].Parts)
                {
                    championParts.Attach();
                }
            }
        }
        
        public void TakeDamage(int damage)
        {
            var prevHealth = Health;
        
            var partsNum = damage;
        
            if (Health - damage < 0)
            {
                partsNum = Health;
            }
        
            DetachParts(partsNum);
            
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                
                var gameProcess = FindObjectOfType<GameProcess>();
                if (!ReferenceEquals(gameProcess, null))
                {
                    gameProcess.ApplyGameState(GameStateEnum.Lose);
                }
            }

            EventRepository.ChampionHealthChanged.Invoke(this, Health, prevHealth);
        }
        
        private void OnGameStateChanged(GameStateEnum gameState)
        {
            if (gameState == GameStateEnum.Result)
            {
                this.enabled = false;
            }
        }
    }
}