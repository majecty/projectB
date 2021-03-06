﻿using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    [RequireComponent (typeof (Slider))]
    public class PlayerHpBar : MonoBehaviour
    {
        private Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            slider.value = (float)Battle.Instance.State.player.Hp / 100;
        }
    }
}
