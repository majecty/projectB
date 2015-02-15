using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Smooth.Algebraics;
using Battle.CoreData;

namespace Battle
{
    public class PlayerTurn : Turn
    {
        public PlayerTurn(State _state, EventReceiver _eventReceiver) : base(_state, _eventReceiver) { }

        public override Run<Unit> StartTurn()
        {
            Run<Unit> _waitingUserClickTurnEnd = WaitingUserClickTurnEnd();

            _waitingUserClickTurnEnd.Then((_unit) => {
                Debug.Log("User clicked " + mState.player.DeckToString());
                return Run<Unit>.Default();
            });

            Run<Unit> _attack = _waitingUserClickTurnEnd.Then((_unit) => Attack());

            Run<Unit> _turnEndMessage = _attack.Then((_unit) => {
                return Run<Unit>.After(0.5f, () => {
                    Debug.Log("Player turn end.");
                    return new Unit();
                });
            });

            Run<Unit> _cleanUpState = _turnEndMessage.Then((_unit) => {
                mState.player.ClearAllClicked();
                return Run<Unit>.Default();
            });

            return _turnEndMessage;
        }

        private Run<Unit> WaitingUserClickTurnEnd()
        {
            var _waitingRun = Run<Unit>.MakeDeferred();
            var _lastCardAnimation = Run<Unit>.Default();

            Action<int, Run<Unit>> _clickedCardHandler;
            _clickedCardHandler = (_clickedCardIndex, _eventAnimation) => {
                _lastCardAnimation = _eventAnimation;
                SetClicked(_clickedCardIndex);
            };
            mEventReceiver.OnClickCardEvent += _clickedCardHandler;

            Action _turnEndButtonHandler;
            _turnEndButtonHandler = () => {
                if (!_lastCardAnimation.IsDone)
                    return;

                Debug.Log("Fire event");
                _waitingRun.Fire(new Unit());
                mEventReceiver.OnClickCardEvent -= _clickedCardHandler;
                mEventReceiver.OnClickTurnEndEvent -= _turnEndButtonHandler;
            };
            mEventReceiver.OnClickTurnEndEvent += _turnEndButtonHandler;

            return _waitingRun;
        }

        private void SetClicked(int _clickedCardIndex)
        {
            Debug.Log("User clicked " + _clickedCardIndex);
            if (mState.player.IsClickedCard(_clickedCardIndex))
            {
                mState.player.UnClickCard(_clickedCardIndex);
            }
            else
            {
                mState.player.ClickCard(_clickedCardIndex);
            }
        }

        private Run<Unit> Attack()
        {
            //FIXME: card's stat should be applied.
            var _deck = mState.player.ClickedCardIndexes;
            var _defaultDamage = 10 * _deck.Count();
            var _criticalAppliedDamage = Critical.Apply(_deck, _defaultDamage);
            mState.enemy.DiminishLife(_criticalAppliedDamage);
            return Run<Unit>.Default();
        }
    }
}
