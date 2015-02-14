using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineHelper : MonoBehaviour
{
	private static CoroutineHelper _instance;
    public static CoroutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                var instanceGameObject = new GameObject("CoroutineHelper");
				instanceGameObject.AddComponent<CoroutineHelper>();
				_instance = instanceGameObject.GetComponent<CoroutineHelper>();
            }
            return _instance;
        }
    }
}
