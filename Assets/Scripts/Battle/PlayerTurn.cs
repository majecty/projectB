using UnityEngine;
using System.Collections;

namespace Battle
{
    public class PlayerTurn : Turn
    {
        public override Run StartTurn()
        {
            return Run.After (1.0f, () => Debug.Log ("Player turn end."));
        }
    }
}