using UnityEngine;
using System;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public class PlayerTurn : Turn
    {
        public PlayerTurn(EventReceiver eventReceiver) : base(eventReceiver) { }

        public override Run<Unit> StartTurn()
        {
            Run<int> userClickedCard = WaitingCardClick();
            Run<Unit> clickedMessage = userClickedCard.Then((clickedNum) => {
                Debug.Log("User clicked " + clickedNum);
                return Run<Unit>.Default();
            });
            Run<Unit> turnEndMessage = clickedMessage.Then((unit) => {
                return Run<Unit>.After(0.5f, () => Debug.Log("Player turn end."));
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
    }
}
