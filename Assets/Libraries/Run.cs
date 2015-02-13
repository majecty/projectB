using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Slinq;
using Smooth.Algebraics;

public class Run
{
    public bool isDone;
    public bool abort;
    private Option<IEnumerator> action;

    private void Start ()
    {
        action.ForEach((actionValue) => CoroutineHelper.Instance.StartCoroutine (actionValue));
    }

    public Coroutine WaitFor
    {
        get
        {
            return CoroutineHelper.Instance.StartCoroutine (_WaitFor (onDone: Option<Action>.None));
        }
    }

    public IEnumerator _WaitFor(Action onDone)
    {
        return _WaitFor(onDone.ToSome());
    }

    public IEnumerator _WaitFor (Option<Action> onDone)
    {
        while (!isDone)
            yield return null;
        if (onDone.isSome)
          onDone.value();
    }

    public void Abort ()
    {
        abort = true;
    }

    public static Run EachFrame (Action action)
    {
        var tmp = new Run ();
        tmp.action = Option.Create(RunEachFrame (tmp, action));
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunEachFrame (Run run, Action action)
    {
        run.isDone = false;
        while (true)
        {
            if (!run.abort)
                action();
            else
                break;
            yield return null;
        }
        run.isDone = true;
    }

    public static Run After (float delay, System.Action action)
    {
        var tmp = new Run ();
        tmp.action = Option.Create(RunAfter(tmp, delay, action));
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunAfter (Run run, float delay, System.Action action)
    {
        run.isDone = false;
        yield return new WaitForSeconds (delay);
        if (!run.abort)
            action();
        run.isDone = true;
    }

    public static Run Coroutine (IEnumerator coroutine)
    {
        var tmp = new Run ();
        tmp.action = RunCoroutine(tmp, coroutine).ToOption();
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunCoroutine (Run run, IEnumerator coroutine)
    {
        yield return CoroutineHelper.Instance.StartCoroutine (coroutine);
        run.isDone = true;
    }

    public Run ExecuteWhenDone (System.Action action)
    {
        var tmp = new Run ();
        tmp.action = _WaitFor (() => {
            action ();
            tmp.isDone = true;
        }).ToSome();
        tmp.Start ();
        return tmp;
    }

    public static Run Join (List<Run> runs)
    {
        var tmp = new Run ();
        tmp.action = WaitForRuns(tmp, runs).ToSome();
        tmp.Start ();
        return tmp;
    }

    public static Run Join (Run r1, Run r2)
    {
        return Join (new List<Run> { r1, r2 });
    }

    private static IEnumerator WaitForRuns (Run run, List<Run> runs)
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
        tmp.action = RunWaitWhile(tmp, predicate).ToSome();
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunWaitWhile (Run run, Func<bool> predicate)
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
        tmp.action = RrunWaitSeconds(tmp, seconds).ToSome();
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RrunWaitSeconds (Run run, float seconds)
    {
        yield return new WaitForSeconds (seconds);
        run.isDone = true;
    }

    public Run Then (Func<Run> nextRunGetter)
    {
        var tmp = new Run ();
        tmp.action = RunThen(tmp, nextRunGetter).ToSome();
        tmp.Start ();
        return tmp;
    }

    private IEnumerator RunThen (Run run, Func<Run> nextRunGetter)
    {
        while (!isDone)
            yield return null;

        var nRun = nextRunGetter ();
        yield return nRun.WaitFor;

        run.isDone = true;
    }
}
