using UnityEngine;
using System.Collections;
using Smooth.Algebraics;

namespace Battle
{
    public abstract class Turn
    {
        public abstract Run<Unit> StartTurn();
    }
}
