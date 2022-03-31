using System.Text.Json;

namespace practice_task_1;

public interface IRecognizable<out T>
{
    public T? Id { get; }
}

public interface IValidatable
{
    public ErrorsDict GetValidationErrors();
}

public interface IFullyModifiable<in T>
{
    public void Modify(T varName);
}

public interface ILookupAble<T>
{
    public bool Contains(T lookupExpr);
    public string[] Keys();
    public Dictionary<string, T> Items();
}

public class Collection<TKeyType, TValueType>
    where TKeyType: IComparable<TKeyType>
    where TValueType : 
    IRecognizable<TKeyType>, 
    IValidatable, 
    IFullyModifiable<Dictionary<string, string>>, 
    ILookupAble<string>,
    new()
{
    // step off the original sequential data structure for sake of speed
    private readonly SortedSet<TKeyType> _keys;
    private List<TValueType> _container;

    public int Size => _container.Count;
    
    public Collection()
    {
        _keys = new SortedSet<TKeyType>();
        _container = new List<TValueType>();
    }
    
    public TValueType this[int i] => _container[i];
    
    public void Add(TValueType value)
    {
        if (value.Id == null || _keys.Contains(value.Id))
            return;

        _keys.Add(value.Id);
        _container.Add(value);
    }

    public bool Remove(TKeyType id)
    {
        if (!_keys.Contains(id)) 
            return false;
        int index = GetIndex(id) ?? -1;
        _keys.Remove(id);
        _container.RemoveAt(index);
        return true;
    }

    public void Clear()
    {
        _keys.Clear();
        _container.Clear();
    }

    public bool Contains(TKeyType id) => _keys.Contains(id);

    public int? GetIndex(TKeyType id)
    {
        if (!Contains(id))
            return null;
        
        for (int i = 0; i < Size; ++i)
        {
            if (Equals(_container[i].Id, id))
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
        _container[index.Value] = value;
        return true;

    }

    public Collection<TKeyType, TValueType> Filter(string? expr = null)
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
    }

    public void Sort(string field)
    {
        _container = _container.OrderBy(
            value => value.GetType().GetProperty(field)?.GetValue(value)
            ).ToList();
    }

    public void DumpIntoJson(string filename)
    {
        using StreamWriter fileStream = new(filename);
        string json = JsonSerializer.Serialize(_container);
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