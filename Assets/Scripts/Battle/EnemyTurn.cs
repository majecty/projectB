using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public class EnemyTurn : Turn
    {

        public override Run<Unit> StartTurn()
        {
            return Run<Unit>.After(1.0f, () => Debug.Log ("Enemy turn end."));
        }
    }
}
