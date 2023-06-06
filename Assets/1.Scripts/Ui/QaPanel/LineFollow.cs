using System;
using UnityEngine;

namespace _1.Scripts.Ui.QaPanel
{
    public class LineFollow : MonoBehaviour
    {
        [SerializeField] private Transform cham;

        private UnityEngine.Camera _camera;
        
        
        
        private void Awake()
        {
            this._camera = UnityEngine.Camera.main;
        }

        private void FixedUpdate()
        {
            var worldToScreenPoint = this._camera.WorldToScreenPoint(this.cham.position);
            worldToScreenPoint.y = 0f;
            this.transform.position = worldToScreenPoint;
        }
    }
}