using UnityEngine;
using UnityEngine.UI;
using System;

namespace Battle.UI
{
    public class LosePopup : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void Set(bool flag)
        {
            image.gameObject.SetActive(flag);
        }
    }
}
