using System.Text.Json;
namespace practice_task_1;


public class VersionedObject<TKeyType, TObject> 
    where TObject: class, IGenericValueType<TKeyType>, new()
{
    private TKeyType? _key;
    private List<MetaDataWrapper<TKeyType, TObject>> _objVersions;

    private int _latestStable = -1;

    // properties
    public MetaDataWrapper<TKeyType, TObject> this[int i] => _objVersions[i];
    public TKeyType? Id => this._key;
    public int size => _objVersions.Count;
    
    public TObject? Value => _latestStable != -1 ? this._objVersions[this._latestStable].Value : null;

    public VersionedObject()
    {
        _key = default;
        _objVersions = new List<MetaDataWrapper<TKeyType, TObject>>();
    }

    public (TKeyType, ErrorsDict) CommitNew(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        var obj = new MetaDataWrapper<TKeyType, TObject>(dict, user);

        TKeyType id = obj.Id;
        ErrorsDict errors = obj.Errors;

        this._key ??= id;
        
        if (this._key != null && !Equals(id, this._key))
            errors.Add("Id", "IdViolation");

        if (errors.Count == 0)
        {
            _objVersions.Add(new MetaDataWrapper<TKeyType, TObject>(dict, user));
            
        }

        return (id, errors);
    }

    public void Approve(int version)
    {
        if (_latestStable != -1)
        {
            _objVersions[_latestStable].Discard();
        }
        
        _objVersions[version].Publish();
        _latestStable = version;
    }

    public void Reject(int version)
    {
        _objVersions[version].Discard();   
    }

    


}


public class Collection<TKeyType, TValueType>
    where TValueType: class, IGenericValueType<TKeyType>, new()
{
    // step off the original sequential data structure for sake of speed
    
    private List<TKeyType> _keys;
    private Dictionary<TKeyType, VersionedObject<TKeyType, TValueType>> _Container;

    public int Size => _keys.Count;

    public Collection()
    {
        _keys = new List<TKeyType>();
        _Container = new Dictionary<TKeyType, VersionedObject<TKeyType, TValueType>>();
    }

    public ErrorsDict Commit(TKeyType id, IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        (_, ErrorsDict errors) = this._Container[id].CommitNew(dict, user);
        return errors;
    }

    public ErrorsDict InitialCommit(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        var versionedObject = new VersionedObject<TKeyType, TValueType>();
        (TKeyType id, ErrorsDict errors) = versionedObject.CommitNew(dict, user);
        
        if (_keys.Contains(id))
            errors.Add("Id", "IdCollision");
        
        if (errors.Count != 0) 
            return errors;
        
        _Container[id] = versionedObject;
        _keys.Add(id);

        return errors;
    }

    public void Clear()
    {
        _keys.Clear();
        _Container.Clear();
    }

    public bool Contains(TKeyType id) => _Container.ContainsKey(id) && _Container[id].Value != null;

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
        _keys.Sort(new VersionedObjectComparer<TKeyType, TValueType>(this._Container, field));
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
    
    /*public Collection<TKeyType, TInnerObject, TValueType> LoadFromJson(string filename)
    {
        var errorCollection = new Collection<TKeyType, TInnerObject, TValueType>();
        
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
    }*/
}