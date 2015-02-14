using UnityEngine;
using UnityEngine.UI;
using System;

namespace Battle.UI
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void Set(bool _flag)
        {
            image.gameObject.SetActive(_flag);
        }
    }
}
