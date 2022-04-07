using System.Text.Json;
namespace practice_task_1;

public class Collection<TKeyType, TValueType>
where TValueType: class, IGenericValueType<TKeyType>, new()
{
    // step off the original sequential data structure for sake of speed
    
    private List<TKeyType> _keys;

    private Dictionary<TKeyType, List<MetaDataWrapper<TKeyType, TValueType>>> _Container;
    private int _commitsCount = 0;

    public int Size => _keys.Count;
    public int CommitsCount => CommitsCount;

    public MetaDataWrapper<TKeyType, TValueType> this[int i] =>
    {
        if ( this._Container[this._keys[i]].Count)
            return _Container[_keys[i]][0];
    } 
    
    public Collection()
    {
        _keys = new List<TKeyType>();
        _Container = new Dictionary<TKeyType, List<MetaDataWrapper<TKeyType, TValueType>>>();
    }

    public ErrorsDict Commit(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    { 
        MetaDataWrapper<TKeyType, TValueType> obj = new(dict, user);
        
        TKeyType? key = obj.Id;
        ErrorsDict errors = obj.Errors;

        if (key == null)
        {
            return errors;
        }

        if (_Container.ContainsKey(key))
        {
            errors.Add("Id", "IdCollision");
        }

        if (errors.Count == 0)
        {
            _Container[key].Add(obj);
            _keys.Add(key);
            _commitsCount += 1;
        }

        return errors;
    }

    public bool Revert(TKeyType id)
    {
        if (!_keys.Contains(id)) 
            return false;
        int index = GetIndex(id) ?? -1;
        
        _keys.RemoveAt(index);
        _Container.Remove(id);
        
        return true;
    }

    public void Clear()
    {
        _keys.Clear();
        _Container.Clear();
    }

    public bool Contains(TKeyType id) => _Container.ContainsKey(id);

    public int? GetIndex(TKeyType id)
    {
        if (!Contains(id))
            return null;
        
        for (int i = 0; i < Size; ++i)
        {
            if (Equals(_Container[_keys[i]].Id, id))
            {
                return i;
            }
        }

        return null;
    }

    public bool Edit(TKeyType id, TValueType value)
    {
        if (Equals(value.Id, id)) 
            return false;
        
        int? index = GetIndex(id);

        if (index is null) 
            return false;
        _Container[index.Value] = value;
        return true;

    }

    /*public Collection<TKeyType, TValueType> Filter(string? expr = null)
    {
        if (string.IsNullOrEmpty(expr))
        {
            return this;
        }
        
        Collection<TKeyType, TValueType> filtered = new();
        foreach (TValueType value in _container)
        {
            if (!value.Contains(expr) || value.Id is null)
                continue;
            
            filtered._keys.Add(value.Id);
            filtered._container.Add(value);
        }

        return filtered;
    }*/

    public void Sort(string field)
    {
        _container = _container.OrderBy(
            value => value.GetType().GetProperty(field)?.GetValue(value)
            ).ToList();
    }

    public void DumpIntoJson(string filename)
    {
        using StreamWriter fileStream = new(filename);
        string json = string.Empty;
        try
        {
            json = JsonSerializer.Serialize(_container);
        }
        catch (NotSupportedException)
        {
            List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();
            foreach (TValueType obj in _container)
            {
                lst.Add(obj.Items());
            }
            json = JsonSerializer.Serialize(lst);
        }
        fileStream.Write(json);
    }
    
    public Collection<TKeyType, TValueType> LoadFromJson(string filename)
    {
        var errorCollection = new Collection<TKeyType, TValueType>();
        
        using StreamReader fileStream = new(filename);
        string json = fileStream.ReadToEnd();
        
        var arr = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

        if (arr == null)
            return errorCollection;
        
        foreach (var dict in arr)
        {
            TValueType value = new(); 
            value.Modify(dict);
            ErrorsDict errors = value.GetValidationErrors();
            
            if (value.Id != null && errors.Count == 0 && !Contains(value.Id))
            {
                Add(value);
            }
            else
            {
                errorCollection.Add(value);
            }
        }
        
        return errorCollection;
    }
}