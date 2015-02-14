using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    using UI;

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
            var turnEndType = TurnEndType.NotEnd;
            while (true)
            {
                var playerTurn = new PlayerTurn(state, eventReceiver);
                var playerTurnRoutine = playerTurn.StartTurn();
                yield return playerTurnRoutine.WaitFor;

                var enemyTurn = new EnemyTurn(state, eventReceiver);
                var enemyTurnRoutine = enemyTurn.StartTurn();
                yield return enemyTurnRoutine.WaitFor;

                Debug.Log("Turn total: " + state);

                turnEndType = CheckEndTurn();
                if (turnEndType != TurnEndType.NotEnd)
                {
                    break;
                }
            }

            Debug.Log("Game End!");
            AnimateGameEnd(turnEndType);
        }

        private TurnEndType CheckEndTurn()
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

        private Run<Unit> AnimateGameEnd(TurnEndType turnEndType)
        {
            switch (turnEndType)
            {
                case TurnEndType.Win:
                    WinPopup winPopup = FindObjectOfType(typeof(WinPopup)) as WinPopup;
                    winPopup.Set(true);
                    return Run<Unit>.After(3.0f, () => { winPopup.Set(false); return new Unit(); });
                case TurnEndType.Lose:
                    LosePopup losePopup = FindObjectOfType(typeof(LosePopup)) as LosePopup;
                    losePopup.Set(true);
                    return Run<Unit>.After(3.0f, () => { losePopup.Set(false); return new Unit(); });
                default:
                    Debug.LogError("Invalid turnEndType: " + turnEndType.ToString());
                    return Run<Unit>.Empty();
            }
        }
    }
}
