using UnityEngine;
using System.Collections;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private EventReceiver eventReceiver;
        Turn currentTurn;

        private void Start()
        {
            StartCoroutine(TurnIterator());
        }

        private IEnumerator TurnIterator()
        {
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
