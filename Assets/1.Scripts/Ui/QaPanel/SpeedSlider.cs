using System;
using UnityEngine;
using UnityEngine.UI;

namespace _1.Scripts.Ui.QaPanel
{
    public class SpeedSlider : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Slider slider;


        private void Update()
        {
            var speed = new Vector3(this.slider.value, 0f, 0f);
            this.rb.velocity = speed;
        }
    }
}