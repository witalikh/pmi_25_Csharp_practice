using System.Collections;

namespace practice_task_1;

public class ErrorsDict : IEnumerable<KeyValuePair<string, LinkedList<string>>>
{
    // container of errors
    private readonly Dictionary<string, LinkedList<string>> _errorsDict = new();

    // properties
    public int Count => _errorsDict.Count;

    public void Add(string key, string value)
    {
        if (_errorsDict.ContainsKey(key) == false)
            _errorsDict[key] = new LinkedList<string>();

        _errorsDict[key].AddLast(value);
    }

    public IEnumerator<KeyValuePair<string, LinkedList<string>>> GetEnumerator()
    {
        return _errorsDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) _errorsDict).GetEnumerator();
    }
}