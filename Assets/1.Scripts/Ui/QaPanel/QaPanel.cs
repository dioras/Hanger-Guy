using System;
using UnityEngine;

namespace _1.Scripts.Ui.QaPanel
{
    public class QaPanel : MonoBehaviour
    {
        private void Awake()
        {
            this.gameObject.SetActive(Debug.isDebugBuild);
        }
    }
}