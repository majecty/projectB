using UnityEngine;
using System.Collections;

namespace Battle
{
    public abstract class Turn
    {
        public abstract Run<Unit> StartTurn();
    }
}
