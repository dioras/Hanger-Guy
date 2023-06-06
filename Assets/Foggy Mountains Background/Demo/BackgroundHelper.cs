using System;
using _1.Scripts.Game;
using _1.Scripts.GameEvent;
using UnityEngine;
using UnityEngine.UI;
using _1.Scripts.Champion;

public class BackgroundHelper : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    
    private float _pos;
    private RawImage _image;
    private float _oPos;

    

    private void Awake()
    {
        EventRepository.GameStateChanged.AddListener(OnGameStateChanged);
        EventRepository.ChampionHealthChanged.AddListener(OnChampionHealthChanged);
    }

    private void Start()
    {
        this._image = GetComponent<RawImage>();
    }
    
    private void Update()
    {
        if (ReferenceEquals(this.rb, null))
        {
            return;
        }
    
        this._pos += (this.rb.transform.position.x - this._oPos) * this.speed;
        this._oPos = this.rb.transform.position.x;

        if (this._pos > 1.0f)
        {
            this._pos -= 1.0f;
        }
        else if (this._pos < 0f)
        {
            this._pos += 1.0f;
        }

        this._image.uvRect = new Rect(this._pos, 0, 1, 1);
    }

    private void OnDestroy()
    {
        EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
    }
    


    private void OnChampionHealthChanged(ChampionHealth championHealth, int health, int prevHealth)
    {
        if (health > 0)
        {
            return;
        }
            
        EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
        EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
        this.enabled = false;
    }

    private void OnGameStateChanged(GameStateEnum gameState)
    {
        if (gameState == GameStateEnum.Play)
        {
            this.rb = FindObjectOfType<Champion>().GetComponent<Rigidbody>();
            this._oPos = this.rb.transform.position.x;
        }
    
        if (gameState == GameStateEnum.Lose || gameState == GameStateEnum.Result)
        {
            EventRepository.GameStateChanged.RemoveListener(OnGameStateChanged);
            EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
            this.enabled = false;
        }
    }
}
