using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public abstract class Turn
    {
        protected EventReceiver eventReceiver;
        protected State state;

        public Turn(State state, EventReceiver eventReceiver)
        {
            this.state = state;
            this.eventReceiver = eventReceiver;
        }

        public abstract Run<Unit> StartTurn();
    }
}
