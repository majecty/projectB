using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public abstract class Turn
    {
        protected EventReceiver eventReceiver;

        public Turn(EventReceiver eventReceiver)
        {
            this.eventReceiver = eventReceiver;
        }

        public abstract Run<Unit> StartTurn();
    }
}
