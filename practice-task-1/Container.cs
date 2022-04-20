using System.Text.Json;
namespace practice_task_1;

public class Collection<TKeyType, TValueType>
    where TValueType: class, IGenericValueType<TKeyType>, new()
{
    // dictionary
    private Dictionary<TKeyType, MetaDataWrapper<TKeyType, TValueType>> _Container;

    public int Size => Keys.Count;
    public MetaDataWrapper<TKeyType, TValueType> this[TKeyType key] => this._Container[key];

    // auto-properties
    public List<TKeyType> Keys { get; }

    public Collection()
    {
        Keys = new List<TKeyType>();
        _Container = new Dictionary<TKeyType, MetaDataWrapper<TKeyType, TValueType>>();
    }

    public ErrorsDict Add(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        var obj = new MetaDataWrapper<TKeyType, TValueType>(dict, user);
        ErrorsDict errors = obj.Errors;
        
        if (Keys.Contains(obj.Id))
            errors.Add("Id", "IdCollision");
        
        if (errors.Count != 0) 
            return errors;
        
        _Container[obj.Id] = obj;
        Keys.Add(obj.Id);

        return errors;
    }
    
    public ErrorsDict Edit(TKeyType id, IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        var obj = new MetaDataWrapper<TKeyType, TValueType>(dict, user);
        ErrorsDict errors = obj.Errors;
        
        if (obj.Id != null && !Equals(id, obj.Id))
            errors.Add("Id", "IdViolation");

        if (errors.Count == 0)
        {
            this._Container[obj.Id] = obj;
        }
        
        return errors;
    }
    
    public bool Delete(TKeyType id)
    {
        if (!this._Container.ContainsKey(id))
            return false;
        
        this._Container.Remove(id);
        this.Keys.Remove(id);
        return true;
    }

    /*public bool Approve(TKeyType id, AbstractUser user)
    {
        return this.Contains(id) && this._Container[id].Publish();
    }
    
    public bool Reject(TKeyType id, AbstractUser user)
    {
        return this.Contains(id) && this._Container[id].Discard();
    }*/

    public void Clear()
    {
        Keys.Clear();
        _Container.Clear();
    }

    public bool Contains(TKeyType id) => _Container.ContainsKey(id);

    public int? GetIndex(TKeyType id)
    {
        if (!Contains(id))
            return null;
        
        for (int i = 0; i < Size; ++i)
        {
            if (Equals(_Container[Keys[i]].Id, id))
            {
                return i;
            }
        }

        return null;
    }

    public Collection<TKeyType, TValueType> Filter(string? expr = null)
    {
        if (string.IsNullOrEmpty(expr))
        {
            return this;
        }
        
        Collection<TKeyType, TValueType> filtered = new();
        foreach ((TKeyType key, var value) in this._Container)
        {
            if (!value.Value.Contains(expr))
                continue;
            
            filtered._Container.Add(key, value);
        }
        return filtered;
    }
    
    

    public void Sort(string field)
    {
        Keys.Sort(new VersionedObjectComparer<TKeyType, TValueType>(this._Container, field));
    }

    public void DumpIntoJson(string filename)
    {
        foreach (TKeyType key in this._Container.Keys)
        {
            using StreamWriter fileStream = new($"{filename}/{key?.ToString()}");
            
            var fancyItems = this._Container[key].Value.FancyItems();
            fancyItems["Author"] = this._Container[key].Author.ToString()!;
            fancyItems["Status"] = this._Container[key].Status.ToString();

            string json = JsonSerializer.Serialize(fancyItems);
            fileStream.Write(json);
        }
    }
    
    public Dictionary<string, ErrorsDict> LoadFromJson(string filename)
    {
        return new();
        /*foreach (TKeyType key in this._Container.Keys)
        {
            using StreamWriter fileStream = new($"{filename}/{key?.ToString()}");
            
            var fancyItems = this._Container[key].Value.FancyItems();
            fancyItems["Author"] = this._Container[key].Author.ToString()!;
            fancyItems["Status"] = this._Container[key].Status.ToString();

            string json = JsonSerializer.Serialize(fancyItems);
            fileStream.Write(json);
        }
        
        var errorCollection = new Dictionary<string, ErrorsDict>();
        
        using StreamReader fileStream = new(filename);
        string json = fileStream.ReadToEnd();
        
        var arr = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

        if (arr == null)
            return errorCollection;
        
        foreach (var dict in arr)
        {
            string author = dict["Author"];
            string status = dict["Status"];

            dict.Remove("Author");
            dict.Remove("Status");
            
            AbstractUser user = new Admin();
            ErrorsDict errors = this.Add(dict, user);
            
            if (errors.Count != 0)
            {
                string id = dict.ContainsKey("Id") ? dict["Id"] : string.Empty;
                if (!errorCollection.ContainsKey(id))
                    errorCollection[id] = errors;
                else
                {
                    // TODO: 
                }
            }
        }
        
        return errorCollection;*/
    }
}