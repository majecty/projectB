using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public class PlayerTurn : Turn
    {
        public override Run<Unit> StartTurn()
        {
            return Run<Unit>.After(1.0f, () => Debug.Log ("Player turn end."));
        }
    }
}
