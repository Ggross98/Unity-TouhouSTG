using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ggross.UI {
    public class SliderValueText : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text value;

        private bool showing = true;

        private void Update() {
            if(showing){

                value.text = slider.value + "";
            }
        }
    }
}

