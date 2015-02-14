using UnityEngine;
using System;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public class PlayerTurn : Turn
    {
        public PlayerTurn(State _state, EventReceiver eventReceiver) : base(_state, eventReceiver) { }

        public override Run<Unit> StartTurn()
        {
            Run<int> _userClickedCard = WaitingCardClick();
            var _clickedMessage = _userClickedCard.Then((_clickedCardIndex) => {
                Debug.Log("User clicked " + _clickedCardIndex);
                return Run<Unit>.Default();
            });

            Run<Unit> _attack = _userClickedCard.Then((_clickedCardIndex) => Attack(_clickedCardIndex));

            Run<Unit> _turnEndMessage = _attack.Then((_unit) => {
                return Run<Unit>.After(0.5f, () => {
                    Debug.Log("Player turn end.");
                    return new Unit();
                });
            });

            return _turnEndMessage;
        }

        private Run<int> WaitingCardClick()
        {
            var _waitingRun = Run<int>.MakeDeferred();
            Action<int> _handler;
            _handler = (num) => {
                _waitingRun.Fire(num);
                mEventReceiver.OnClickCardEvent -= _handler;
            };
            mEventReceiver.OnClickCardEvent += _handler;
            return _waitingRun;
        }

        private Run<Unit> Attack(int cardNum)
        {
            mState.enemy.DiminishLife(1);
            return Run<Unit>.Default();
        }
    }
}
