using UnityEngine;
using System;
using System.Collections;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private EventReceiver eventReceiver;

        private void Start()
        {
            var state = new State();
            StartCoroutine(TurnIterator(state));
        }

        private IEnumerator TurnIterator(State state)
        {
            Turn currentTurn;
            while (true)
            {
                currentTurn = new PlayerTurn(state, eventReceiver);
                var playerTurnRoutine = currentTurn.StartTurn();
                yield return playerTurnRoutine.WaitFor;

                currentTurn = new EnemyTurn(state, eventReceiver);
                var enemyTurnRoutine = currentTurn.StartTurn();
                yield return enemyTurnRoutine.WaitFor;

                Debug.Log("Turn total: " + state);
            }
        }
    }
}
