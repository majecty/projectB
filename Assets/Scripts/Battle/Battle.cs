using UnityEngine;
using System.Collections;
using Smooth.Algebraics;
using Battle.UI;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private EventReceiver mEventReceiver;
        public EventReceiver EventReceiver { get { return mEventReceiver; } }

        enum TurnEndType
        {
            WIN,
            LOSE,
            NOTEND
        }

        private static Battle mInstance;
        public static Battle Instance { get { return mInstance; } }

        private State mState;
        public State State { get { return mState; } }

        private void Awake()
        {
            mInstance = this;
            mState = new State();
            StartCoroutine(TurnIterator(mState));
        }

        private IEnumerator TurnIterator(State _state)
        {
            var _turnEndType = TurnEndType.NOTEND;
            while (true)
            {
                var playerTurn = new PlayerTurn(_state, mEventReceiver);
                var playerTurnRoutine = playerTurn.StartTurn();
                yield return playerTurnRoutine.WaitFor;

                var enemyTurn = new EnemyTurn(_state, mEventReceiver);
                var enemyTurnRoutine = enemyTurn.StartTurn();
                yield return enemyTurnRoutine.WaitFor;

                Debug.Log("Turn total: " + _state);

                _turnEndType = CheckEndTurn();
                if (_turnEndType != TurnEndType.NOTEND)
                {
                    break;
                }
            }

            Debug.Log("Game End!");
            AnimateGameEnd(_turnEndType);
        }

        private TurnEndType CheckEndTurn()
        {
            TurnEndType _endType;
            if (mState.player.Hp <= 0)
            {
                _endType = TurnEndType.LOSE;
                Debug.Log("Lose");
            }
            else if (mState.enemy.Hp <= 0)
            {
                _endType = TurnEndType.WIN;
                Debug.Log("Win");
            }
            else
            {
                _endType = TurnEndType.NOTEND;
            }
            return _endType;
        }

        private Run<Unit> AnimateGameEnd(TurnEndType _turnEndType)
        {
            switch (_turnEndType)
            {
                case TurnEndType.WIN:
                    WinPopup _winPopup = FindObjectOfType(typeof(WinPopup)) as WinPopup;
                    _winPopup.Set(true);
                    return Run<Unit>.After(3.0f, () => { _winPopup.Set(false); return new Unit(); });
                case TurnEndType.LOSE:
                    LosePopup _losePopup = FindObjectOfType(typeof(LosePopup)) as LosePopup;
                    _losePopup.Set(true);
                    return Run<Unit>.After(3.0f, () => { _losePopup.Set(false); return new Unit(); });
                default:
                    Debug.LogError("Invalid turnEndType: " + _turnEndType.ToString());
                    return Run<Unit>.Default();
            }
        }
    }
}
