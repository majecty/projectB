using UnityEngine;
using System;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public class PlayerTurn : Turn
    {
        public PlayerTurn(State state, EventReceiver eventReceiver) : base(state, eventReceiver) { }

        public override Run<Unit> StartTurn()
        {
            Run<int> userClickedCard = WaitingCardClick();
            Run<int> clickedMessage = userClickedCard.Then((clickedNum) => {
                Debug.Log("User clicked " + clickedNum);
                return Run<int>.Identity(clickedNum);
            });

            Run<Unit> attack = clickedMessage.Then((clickedNum) => Attack(clickedNum));

            Run<Unit> turnEndMessage = attack.Then((unit) => {
                return Run<Unit>.After(0.5f, () => {
                    Debug.Log("Player turn end.");
                    return new Unit();
                });
            });

            return turnEndMessage;
        }

        private Run<int> WaitingCardClick()
        {
            var waitingRun = Run<int>.MakeDeferred();
            Action<int> handler;
            handler = (num) => {
                waitingRun.Fire(num);
                eventReceiver.OnClickCardEvent -= handler;
            };
            eventReceiver.OnClickCardEvent += handler;
            return waitingRun;
        }

        private Run<Unit> Attack(int cardNum)
        {
            state.Enemy.DiminishLife(1);
            return Run<Unit>.Empty();
        }
    }
}
