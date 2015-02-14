using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Battle.UI
{
    [RequireComponent (typeof (Slider))]
    public class EnemyHpBar : MonoBehaviour
    {
        private Slider mSlider;

        private void Start()
        {
            mSlider = GetComponent<Slider>();
        }

        private void Update()
        {
            mSlider.value = (float)Battle.Instance.State.enemy.Hp / 100;
        }
    }
}
