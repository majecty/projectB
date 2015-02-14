using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Battle
{
    [RequireComponent (typeof (Slider))]
    public class EnemyHpBar : MonoBehaviour
    {
        private Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            slider.value = (float)Battle.Instance.State.Enemy.Hp / 100;
        }
    }
}
