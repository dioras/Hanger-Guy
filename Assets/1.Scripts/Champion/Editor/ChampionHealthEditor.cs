using UnityEditor;
using UnityEngine;

namespace _1.Scripts.Champion.Editor
{
    [CustomEditor(typeof(ChampionHealth))]
    public class ChampionHealthEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var championHealth = (ChampionHealth) this.target;
            if (GUILayout.Button("Cut Part"))
            {
                championHealth.TakeDamage(1);
            }
        }
    }
}