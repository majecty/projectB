using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Battle.UI
{
    [RequireComponent (typeof (Button))]
    public class Card : MonoBehaviour
    {
        [SerializeField] private Image selectedImage;
        [SerializeField] private int cardIndex;
        private Button mButton;

        private void Start()
        {
            mButton = GetComponent<Button>();
            mButton.onClick.AddListener(() => Battle.Instance.EventReceiver.OnClickCard(cardIndex));
        }

        private void Update()
        {
            var _state = Battle.Instance.State;
            var _isClicked = _state.player.IsClickedCard(cardIndex);
            selectedImage.gameObject.SetActive(_isClicked);
        }
    }
}
