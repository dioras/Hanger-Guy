using System.Collections;
using _1.Scripts.GameEvent;
using UnityEngine;
using _1.Scripts.Skins;

namespace _1.Scripts.Champion
{
    public class ChampionMaterial : MonoBehaviour
    {
        [SerializeField] private Texture[] textures;
        
        [SerializeField, Space] private ChampionHealth championHealth;
        [SerializeField]
        private Renderer[] renderers;

        private int currTextureIdx;


        public void ApplyTexture(int index)
        {
            // foreach (var renderer in this.renderers)
            // {
            //     var material = renderer.material;
            //
            //     if (material.mainTexture != this.textures[index])
            //     {
            //         material.mainTexture = this.textures[index];
            //     }
            // }
            //
            // this.currTextureIdx = index;
        }

        

        private void Awake()
        {
            if (PlayerPrefs.GetInt("current_level") < 4) // 0, 1, 2, 3 (4)
            {
                return;
            }

            EventRepository.ChampionHealthChanged.AddListener(OnChampionHealthChanged);
        }

        private void Start()
        {
            ApplyTexture(0);
        }

        private void OnDestroy()
        {
            if (PlayerPrefs.GetInt("current_level") < 4) // 0, 1, 2, 3 (4)
            {
                return;
            }
            
            EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
        }



        public void ChangeTransparencySmoothly(float duration, float startAlpha, float targetAlpha, float transitionTime)
        {
            StartCoroutine(ChangeTransparencySmoothlyProcess(duration, startAlpha, targetAlpha, transitionTime));
        }
        
        private IEnumerator ChangeTransparencySmoothlyProcess(float duration, float startAlpha, float targetAlpha, float transitionTime)
        {
            var t = true;
        
            while (duration > 0f)
            {
                var currTransitionTime = transitionTime;

                var startColor = this.renderers[0].material.color;

                startColor.a = t ? startAlpha : targetAlpha;

                var targetColor = startColor;

                targetColor.a = !t ? startAlpha : targetAlpha;
                
                while (currTransitionTime > 0f)
                {
                    var lerp = 1 - currTransitionTime / transitionTime;

                    foreach (var renderer in this.renderers)
                    {
                        renderer.material.color = Color.Lerp(startColor, targetColor, lerp);
                    }
                
                    currTransitionTime -= Time.deltaTime;
                    duration -= Time.deltaTime;

                    yield return null;
                }

                t = !t;
            
                duration -= Time.deltaTime;

                yield return null;
            }
            
            foreach (var renderer in this.renderers)
            {
                var color = renderer.material.color;
                color.a = startAlpha;
                renderer.material.color = color;
            }
        }
        
        
        private void OnChampionHealthChanged(ChampionHealth championHealth, int newHealth, int prevHealth)
        {
            var partsNum = this.championHealth.ChampionPartsNum;

            if (newHealth > partsNum)
            {
                ApplyTexture(0);
            
                return;
            }

            var ratio = newHealth / (float)partsNum;

            if (ratio >= 0.75f)
            {
                ApplyTexture(0);
            }
            else if (ratio < 0.75f && ratio >= 0.5f)
            {
                ApplyTexture(1);
            }
            else if (ratio < 0.5f && ratio >= 0.25f)
            {
                ApplyTexture(2);
            }
            else if (ratio < 0.25f)
            {
                ApplyTexture(3);
            }
        }
    }
}