using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public class EnemyTurn : Turn
    {
        public EnemyTurn(State state, EventReceiver eventReceiver) : base(state, eventReceiver) { }

        public override Run<Unit> StartTurn()
        {
            var _attack = Attack();
            var turnEndMessage = _attack.Then((unit) => {
                return Run<Unit>.After(1.0f, () => {
                    Debug.Log ("Enemy turn end.");
                    return new Unit();
                });
            });

            return turnEndMessage;
        }

        private Run<Unit> Attack()
        {
            mState.player.DiminishLife(10);
            return Run<Unit>.Default();
        }
    }
}
