using UnityEngine;
using System;
using System.Collections;
using Smooth.Algebraics;

public class EventReceiver : MonoBehaviour
{
    public event Action<int, Run<Unit>> OnClickCardEvent;
    public void OnClickCard(int _clickedCardIndex, Run<Unit> _eventAnimation)
    {
        // c# event keyward need null check.
        if (OnClickCardEvent != null)
            OnClickCardEvent(_clickedCardIndex, _eventAnimation);
        else
            Debug.Log("There is no binded event handler.");
    }

    public event Action OnClickTurnEndEvent;
    public void OnClickTurnEnd()
    {
        // c# event keyward need null check.
        if (OnClickTurnEndEvent != null)
            OnClickTurnEndEvent();
        else
            Debug.Log("There is nos binded event handler");
    }
}
