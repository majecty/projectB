using UnityEngine;
using System.Collections;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private EventReceiver eventReceiver;

        private static Battle instance;
        public static Battle Instance { get { return instance; } }

        private State state;
        public State State { get { return state; } }

        private void Awake()
        {
            instance = this;
            state = new State();
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
