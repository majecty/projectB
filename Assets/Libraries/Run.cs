using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Slinq;
using Smooth.Algebraics;

public class Run<T>
{
    public bool IsDone { get; private set; }

    public bool abort;
    private Option<IEnumerator> mAction;
    T mReturnValue;

    private void Start ()
    {
        mAction.ForEach((actionValue) => CoroutineHelper.Instance.StartCoroutine (actionValue));
    }

    public Coroutine WaitFor
    {
        get
        {
            return CoroutineHelper.Instance.StartCoroutine (_WaitFor (_onDone: Option<Action>.None));
        }
    }

    public IEnumerator _WaitFor(Action _onDone)
    {
        return _WaitFor(_onDone.ToSome());
    }

    public IEnumerator _WaitFor (Option<Action> _onDone)
    {
        while (!IsDone)
            yield return null;
        if (_onDone.isSome)
          _onDone.value();
    }

    public void Abort ()
    {
        abort = true;
    }

    public void Fire(T _value)
    {
        IsDone = true;
        mReturnValue = _value;
    }

    public static Run<T> Default()
    {
        return After(0, () => default(T));
    }

    public static Run<T> Identity(T _input)
    {
        return After(0, () => _input);
    }

    public static Run<Unit> EachFrame (System.Action _action)
    {
        var tmp = new Run<Unit>();
        tmp.mAction = RunEachFrame(tmp, _action).ToSome();
        tmp.mReturnValue = new Unit();
        tmp.Start ();
        return tmp;
    }

    private static IEnumerator RunEachFrame (Run<Unit> _run, Action _action)
    {
        _run.IsDone = false;
        while (true)
        {
            if (!_run.abort)
                 _action();
            else
                break;
            yield return null;
        }
        _run.IsDone = true;
    }

    public static Run<T> After(float _delay, Func<T> _func)
    {
        var _tmp = new Run<T>();
        _tmp.mAction = RunAfter(_tmp, _delay, _func).ToSome();
        _tmp.Start ();
        return _tmp;
    }

    private static IEnumerator RunAfter (Run<T> _run, float _delay, Func<T> _func)
    {
        _run.IsDone = false;
        yield return new WaitForSeconds (_delay);
        if (!_run.abort)
            _run.mReturnValue = _func();
        _run.IsDone = true;
    }

    private static IEnumerator RunWaitWhile (Run<T> _run, Func<bool> _predicate)
    {
        while (!_run.abort &&  _predicate())
        {
            yield return null;
        }
        _run.IsDone = true;
    }

    public static Run<Unit> WaitSeconds (float _seconds)
    {
        var _tmp = new Run<Unit>();
        _tmp.mAction = RunWaitSeconds (_tmp, _seconds).ToSome();
        _tmp.Start ();
        return _tmp;
    }

    private static IEnumerator RunWaitSeconds (Run<Unit> _run, float _seconds)
    {
        yield return new WaitForSeconds (_seconds);
        _run.IsDone = true;
    }

    public static Run<T> MakeDeferred()
    {
        var newRun = new Run<T>();
        return newRun;
    }

    public Run<R> Then<R>(Func<T, Run<R>> _then)
    {
        if (IsDone)
        {
            return _then(mReturnValue);
        }

        var _newRun = new Run<R>();
        _newRun.mAction = RunThen(_manager: _newRun, _previousRun: this, _then: _then).ToSome();
        _newRun.Start();
        return _newRun;
    }

    private static IEnumerator RunThen<R>(Run<R> _manager, Run<T> _previousRun, Func<T, Run<R>> _then)
    {
        while (!_manager.abort && !_previousRun.IsDone)
        {
            yield return null;
        }

        var _nextRun = _then(_previousRun.mReturnValue);

        while (!_manager.abort && !_nextRun.IsDone)
        {
            yield return null;
        }

        _manager.IsDone = true;
        _manager.mReturnValue = _nextRun.mReturnValue;
    }
}
