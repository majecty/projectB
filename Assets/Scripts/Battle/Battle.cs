using UnityEngine;
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
            while (true)
            {
                var playerTurn = new PlayerTurn(state, eventReceiver);
                var playerTurnRoutine = playerTurn.StartTurn();
                yield return playerTurnRoutine.WaitFor;

                var enemyTurn = new EnemyTurn(state, eventReceiver);
                var enemyTurnRoutine = enemyTurn.StartTurn();
                yield return enemyTurnRoutine.WaitFor;

                Debug.Log("Turn total: " + state);
            }
        }
    }
}
