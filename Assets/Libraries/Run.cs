using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq;

public class Run<T>
{
    private bool isDone = false;
    public bool IsDone
    {
        get
        {
            return isDone;
        }
        private set
        {
            isDone = value;
        }
    }

    public bool abort;
    private Option<IEnumerator> action;
    T returnValue;

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
        while (!IsDone)
            yield return null;
        if (onDone.isSome)
          onDone.value();
    }

    public void Abort ()
    {
        abort = true;
    }

    public void Fire(T value)
    {
        IsDone = true;
        returnValue = value;
    }

    public static Run<Unit> Empty()
    {
        return Run<Unit>.After(0, () => new Unit());
    }

    public static Run<T> Default()
    {
        return After(0, () => default(T));
    }

    public static Run<T> Identity(T input)
    {
        return After(0, () => input);
    }

    public static Run<Unit> EachFrame (System.Action action)
    {
        var tmp = new Run<Unit>();
        tmp.action = RunEachFrame(tmp, action).ToSome();
        tmp.returnValue = new Unit();
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunEachFrame (Run<Unit> run, Action action)
    {
        run.IsDone = false;
        while (true)
        {
            if (!run.abort)
                action();
            else
                break;
            yield return null;
        }
        run.IsDone = true;
    }

    public static Run<T> After(float delay, Func<T> func)
    {
        var tmp = new Run<T>();
        tmp.action = RunAfter(tmp, delay, func).ToSome();
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunAfter (Run<T> run, float delay, Func<T> func)
    {
        run.IsDone = false;
        yield return new WaitForSeconds (delay);
        if (!run.abort)
            run.returnValue = func();
        run.IsDone = true;
    }

    private static IEnumerator RunWaitWhile (Run<T> run, Func<bool> predicate)
    {
        while (!run.abort &&  predicate())
        {
            yield return null;
        }
        run.IsDone = true;
    }

    public static Run<Unit> WaitSeconds (float seconds)
    {
        var tmp = new Run<Unit>();
        tmp.action = RunWaitSeconds (tmp, seconds).ToSome();
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunWaitSeconds (Run<Unit> run, float seconds)
    {
        yield return new WaitForSeconds (seconds);
        run.IsDone = true;
    }

    public static Run<T> MakeDeferred()
    {
        var newRun = new Run<T>();
        return newRun;
    }

    public Run<R> Then<R>(Func<T, Run<R>> then)
    {
        if (isDone)
        {
            return then(returnValue);
        }

        var newRun = new Run<R>();
        newRun.action = RunThen(manager: newRun, previousRun: this, then: then).ToSome();
        newRun.Start();
        return newRun;
    }

    private static IEnumerator RunThen<R>(Run<R> manager, Run<T> previousRun, Func<T, Run<R>> then)
    {
        while (!manager.abort && !previousRun.isDone)
        {
            yield return null;
        }

        var nextRun = then(previousRun.returnValue);

        while (!manager.abort && !nextRun.isDone)
        {
            yield return null;
        }

        manager.isDone = true;
        manager.returnValue = nextRun.returnValue;
    }
}
