using System.Collections.Generic;

public static class DebugExtension
{
    public static string DebugToString<T>(this IEnumerable<T> _enumerable)
    {
        var _log = "";
        foreach (var element in _enumerable)
        {
            _log += element.ToString();
            _log += ", ";
        }

        return _log;
    }
}
