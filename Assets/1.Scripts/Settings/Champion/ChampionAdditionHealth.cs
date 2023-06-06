using UnityEngine;

namespace _1.Scripts.Settings.Champion
{
    [CreateAssetMenu(fileName = "Champion Addition Health", menuName = "Settings/Champion", order = 0)]
    public class ChampionAdditionHealth : ScriptableObject
    {
        public int Level1 => this.level1;
        public int Level2 => this.level2;
        public int Level3 => this.level3;
        public int Level4 => this.level4;
        public int Other => this.other;
        
        [SerializeField] private int level1;
        [SerializeField] private int level2;
        [SerializeField] private int level3;
        [SerializeField] private int level4;
        [SerializeField] private int other;
    }
}