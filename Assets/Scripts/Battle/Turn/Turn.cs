using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public abstract class Turn
    {
        protected EventReceiver mEventReceiver;
        protected State mState;

        public Turn(State _state, EventReceiver _eventReceiver)
        {
            this.mState = _state;
            this.mEventReceiver = _eventReceiver;
        }

        public abstract Run<Unit> StartTurn();
    }
}
