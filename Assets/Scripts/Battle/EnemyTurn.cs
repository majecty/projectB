using UnityEngine;
using System.Collections;

namespace Battle
{
    public class EnemyTurn : Turn
    {
        public override Run StartTurn()
        {
            return Run.After (1.0f, () => Debug.Log ("Enemy turn end."));
        }
    }
}