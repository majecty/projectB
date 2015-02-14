using UnityEngine;
using System;
using System.Collections;

public class EventReceiver : MonoBehaviour
{
	public event Action<int> OnClickCardEvent;
	public void OnClickCard(int cardNum)
	{
		OnClickCardEvent(cardNum);
	}
}
