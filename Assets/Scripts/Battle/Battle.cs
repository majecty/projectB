using UnityEngine;
using System.Collections;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private EventReceiver eventReceiver;

        private void Start()
        {
            StartCoroutine(TurnIterator());
        }

        private IEnumerator TurnIterator()
        {
            Turn currentTurn;
            while (true)
            {
                currentTurn = new PlayerTurn(eventReceiver);
                var playerTurnRoutine = currentTurn.StartTurn();
                yield return playerTurnRoutine.WaitFor;

                currentTurn = new EnemyTurn(eventReceiver);
                var enemyTurnRoutine = currentTurn.StartTurn();
                yield return enemyTurnRoutine.WaitFor;
            }
        }
    }
}
