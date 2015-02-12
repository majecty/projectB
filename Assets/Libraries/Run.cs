using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Slinq;

public class Run
{
    public bool isDone;
    public bool abort;
    private IEnumerator action;

#region Run.EachFrame
    public static Run EachFrame (System.Action action)
    {
        var tmp = new Run ();
        tmp.action = _RunEachFrame (tmp, action);
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator _RunEachFrame (Run run, System.Action action)
    {
        run.isDone = false;
        while (true)
        {
            if (!run.abort && action != null)
                action ();
            else
                break;
            yield return null;
        }
        run.isDone = true;
    }
#endregion Run.EachFrame

#region Run.After
    public static Run After (float delay, System.Action action)
    {
        var tmp = new Run ();
        tmp.action = _RunAfter (tmp, delay, action);
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator _RunAfter (Run run, float delay, System.Action action)
    {
        run.isDone = false;
        yield return new WaitForSeconds (delay);
        if (!run.abort && action != null)
            action ();
        run.isDone = true;
    }
#endregion Run.After

#region Run.Coroutine
    public static Run Coroutine (IEnumerator coroutine)
    {
        var tmp = new Run ();
        tmp.action = _Coroutine (tmp, coroutine);
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator _Coroutine (Run run, IEnumerator coroutine)
    {
        yield return CoroutineHelper.Instance.StartCoroutine (coroutine);
        run.isDone = true;
    }
#endregion Run.Coroutine

    private void Start ()
    {
        if (action != null)
            CoroutineHelper.Instance.StartCoroutine (action);
    }

    public Coroutine WaitFor
    {
        get
        {
            return CoroutineHelper.Instance.StartCoroutine (_WaitFor (null));
        }
    }

    public IEnumerator _WaitFor (System.Action onDone)
    {
        while (!isDone)
            yield return null;
        if (onDone != null)
            onDone ();
    }

    public void Abort ()
    {
        abort = true;
    }

    public Run ExecuteWhenDone (System.Action action)
    {
        var tmp = new Run ();
        tmp.action = _WaitFor (() => {
            action ();
            tmp.isDone = true;
        });
        tmp.Start ();
        return tmp;
    }

    public static Run Join (List<Run> runs)
    {
        var tmp = new Run ();
        tmp.action = _WaitForRuns (tmp, runs);
        tmp.Start ();
        return tmp;
    }

    public static Run Join (Run r1, Run r2)
    {
        return Join (new List<Run> { r1, r2 });
    }

    private static IEnumerator _WaitForRuns (Run run, List<Run> runs)
    {
        var remainCount = runs.Count;

        runs.ForEach ((newRun) => {
            newRun.ExecuteWhenDone (() => {
                remainCount -= 1;
            });
        });

        Func<bool> isComplete = () => remainCount != 0;
        while (isComplete())
        {
            yield return null;
        }
        run.isDone = true;
    }

    public static Run WaitWhile (Func<bool> predicate)
    {
        var tmp = new Run ();
        tmp.action = _WaitWhile (tmp, predicate);
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator _WaitWhile (Run run, Func<bool> predicate)
    {
        while (predicate())
        {
            yield return null;
        }
        run.isDone = true;
    }

    public static Run WaitSeconds (float seconds)
    {
        var tmp = new Run ();
        tmp.action = _WaitSeconds (tmp, seconds);
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator _WaitSeconds (Run run, float seconds)
    {
        yield return new WaitForSeconds (seconds);
        run.isDone = true;
    }

    public Run Then (Run nextRun)
    {
        var tmp = new Run ();
        tmp.action = _Then (tmp, nextRun);
        tmp.Start ();
        return tmp;
    }

    private IEnumerator _Then (Run run, Run nextRun)
    {
        while (!isDone)
            yield return null;

        nextRun.Start ();
        yield return nextRun.WaitFor;

        run.isDone = true;
    }

    public Run Then (Func<Run> nextRunGetter)
    {
        var tmp = new Run ();
        tmp.action = _Then (tmp, nextRunGetter);
        tmp.Start ();
        return tmp;
    }

    private IEnumerator _Then (Run run, Func<Run> nextRunGetter)
    {
        while (!isDone)
            yield return null;

        var nRun = nextRunGetter ();
        yield return nRun.WaitFor;

        run.isDone = true;
    }
}
