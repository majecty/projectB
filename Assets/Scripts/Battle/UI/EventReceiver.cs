using UnityEngine;
using System;
using System.Collections;

public class EventReceiver : MonoBehaviour
{
    public event Action<int> OnClickCardEvent;
    public void OnClickCard(int clickedCardIndex)
    {
        // c# evenet keyward need null check.
        if (OnClickCardEvent != null)
            OnClickCardEvent(clickedCardIndex);
        else
            Debug.Log("There is no binded event handler.");
    }
}
