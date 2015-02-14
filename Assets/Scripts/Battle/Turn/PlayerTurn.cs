using UnityEngine;
using System;
using System.Collections.Generic;
using Smooth.Algebraics;

namespace Battle
{
    public class PlayerTurn : Turn
    {
        public PlayerTurn(State _state, EventReceiver eventReceiver) : base(_state, eventReceiver) { }

        public override Run<Unit> StartTurn()
        {
            Run<List<int>> _userClickedCardIndexes = WaitingCardClick();
            var _clickedMessage = _userClickedCardIndexes.Then((_clickedCardIndexes) => {
                Debug.Log("User clicked " + _clickedCardIndexes);
                return Run<Unit>.Default();
            });

            Run<Unit> _attack = _userClickedCardIndexes.Then((_clickedCardIndexes) => Attack(_clickedCardIndexes));

            Run<Unit> _turnEndMessage = _attack.Then((_unit) => {
                return Run<Unit>.After(0.5f, () => {
                    Debug.Log("Player turn end.");
                    return new Unit();
                });
            });

            return _turnEndMessage;
        }

        private Run<List<int>> WaitingCardClick()
        {
            var _waitingRun = Run<List<int>>.MakeDeferred();
            var _clickedIndexes = new List<int>();

            Action<int> _clickedCardHandler;
            _clickedCardHandler = (_clickedCardIndex) => _clickedIndexes.Add(_clickedCardIndex);
            mEventReceiver.OnClickCardEvent += _clickedCardHandler;

            Action _turnEndButtonHandler;
            _turnEndButtonHandler = () => {
                _waitingRun.Fire(_clickedIndexes);
                mEventReceiver.OnClickCardEvent -= _clickedCardHandler;
                mEventReceiver.OnClickTurnEndEvent -= _turnEndButtonHandler;
            };
            mEventReceiver.OnClickTurnEndEvent += _turnEndButtonHandler;

            return _waitingRun;
        }

        private Run<Unit> Attack(List<int> cardIndexes)
        {
            //FIXME: card's stat should be applied.
            mState.enemy.DiminishLife(10 * cardIndexes.Count);
            return Run<Unit>.Default();
        }
    }
}
