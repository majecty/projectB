using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Smooth.Algebraics;

namespace Battle.UI
{
    [RequireComponent (typeof (Animator))]
    [RequireComponent (typeof (Button))]
    public class Card : MonoBehaviour
    {
        [SerializeField] private Image selectedImage;
        [SerializeField] private int cardIndex;
        private Animator mAnimator;
        private Button mButton;

        private void Start()
        {
            mAnimator = GetComponent<Animator>();
            mButton = GetComponent<Button>();
            mButton.onClick.AddListener(() => {
                var _animation = StartClickAnimation();
                Battle.Instance.EventReceiver.OnClickCard(cardIndex, _animation);
            });
        }

        private void Update()
        {
            selectedImage.gameObject.SetActive(IsClicked());
        }

        private Run<Unit> StartClickAnimation()
        {
            if (!IsClicked() && IsIdleState())
            {
                mAnimator.SetTrigger("CardSelectTrigger");
                //FIXME: need to get next animation length;
                const float animationLength = 1.0f;
                return Run<Unit>.After(animationLength, () => new Unit());
            }
            else
            {
                return Run<Unit>.Default();
            }
        }

        private bool IsIdleState()
        {
            var animatorState = mAnimator.GetCurrentAnimatorStateInfo(0);
            return animatorState.IsName("Idle");
        }

        private bool IsClicked()
        {
            var _state = Battle.Instance.State;
            return _state.player.IsClickedCard(cardIndex);
        }
    }
}
