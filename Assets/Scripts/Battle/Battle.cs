using UnityEngine;
using System.Collections;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        Turn currentTurn;

        private void Start()
        {
            StartCoroutine(TurnIterator());
        }

        private IEnumerator TurnIterator()
        {
            while (true)
            {
                currentTurn = new PlayerTurn();
                var playerTurnRoutine = currentTurn.StartTurn();
                yield return playerTurnRoutine.WaitFor;

                currentTurn = new EnemyTurn();
                var enemyTurnRoutine = currentTurn.StartTurn();
                yield return enemyTurnRoutine.WaitFor;
            }
        }
    }
}