using UnityEngine;
using System;
using System.Collections;

public class EventReceiver : MonoBehaviour
{
    public event Action<int> OnClickCardEvent;
    public void OnClickCard(int cardNum)
    {
        // c# evenet keyward need null check.
        if (OnClickCardEvent != null)
            OnClickCardEvent(cardNum);
        else
            Debug.Log("There is no binded event handler.");
    }
}
