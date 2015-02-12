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

    private List<Run> runners = new List<Run>();
    public int ScheduledOnGUIItems
    {
        get { return runners.Count; }
    }
    public Run Add(Run aRun)
    {
        if (aRun != null)
            runners.Add (aRun);
        return aRun;
    }

    void Update()
    {
        for(int i = runners.Count-1; i >= 0; i--)
        {
            if (runners[i].isDone)
            runners.RemoveAt(i);
        }
    }
}
