using UnityEditor;
using UnityEngine;

namespace _1.Scripts.Game.Editor
{
    [CustomEditor(typeof(GameProcess))]
    public class GameProcessEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
            {
                return;
            }

            if (GUILayout.Button("Win"))
            {
                (this.target as GameProcess)?.ApplyGameState(GameStateEnum.Result);
            }
        }
    }
}