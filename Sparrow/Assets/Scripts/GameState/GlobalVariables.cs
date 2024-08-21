using System.Collections.Generic;

public static class GlobalVariables
{
    private static readonly object _lockObject = new object();
    private static Dictionary<string, object> _variablesDictionary = new Dictionary<string, object>();

    public static Dictionary<string, object> VariablesDictionary => _variablesDictionary;

    public static Dictionary<string, object> GetAll()
    {
        return _variablesDictionary;
    }

    public static T Get<T>(string key)
    {
        if (_variablesDictionary == null || !_variablesDictionary.ContainsKey(key))
        {
            return default;
        }

        return (T)_variablesDictionary[key];
    }

    public static void Set(string key, object value)
    {
        lock(_lockObject)
        {
            if (_variablesDictionary == null)
            {
                _variablesDictionary = new Dictionary<string, object>();
            }
        }
        _variablesDictionary[key] = value;
    }
}
