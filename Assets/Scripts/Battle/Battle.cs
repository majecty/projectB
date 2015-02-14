using UnityEngine;
using System.Collections;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private EventReceiver eventReceiver;

        enum TurnEndType
        {
            Win,
            Lose,
            NotEnd
        }

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

                var turnEndType = EndTurn();
                if (turnEndType != TurnEndType.NotEnd)
                {
                    break;
                }
            }

            Debug.Log("Game End!");
        }

        private TurnEndType EndTurn()
        {
            TurnEndType endType;
            if (state.Player.Hp <= 0)
            {
                endType = TurnEndType.Lose;
                Debug.Log("Lose");
            }
            else if (state.Enemy.Hp <= 0)
            {
                endType = TurnEndType.Win;
                Debug.Log("Win");
            }
            else
            {
                endType = TurnEndType.NotEnd;
            }
            return endType;
        }
    }
}
