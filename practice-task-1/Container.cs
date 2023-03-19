using System.Text.Json;
using System.Text.Json.Nodes;

namespace practice_task_1;

public class Collection<TKeyType, TValueType>
    where TValueType: class, IGenericValueType<TKeyType>, new()
{
    private Dictionary<TKeyType, MetaDataWrapper<TKeyType, TValueType>> _Container;

    public int Size => Keys.Count;
    public MetaDataWrapper<TKeyType, TValueType> this[TKeyType key] => this._Container[key];
    
    public List<TKeyType> Keys { get; }

    public Collection()
    {
        Keys = new List<TKeyType>();
        _Container = new Dictionary<TKeyType, MetaDataWrapper<TKeyType, TValueType>>();
    }

    public ErrorsDict Add(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        MetaDataWrapper<TKeyType, TValueType> obj = new MetaDataWrapper<TKeyType, TValueType>(dict, user);
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
        MetaDataWrapper<TKeyType, TValueType> obj = new MetaDataWrapper<TKeyType, TValueType>(dict, user);
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
    
    public bool Contains(TKeyType id) => _Container.ContainsKey(id);

    public Collection<TKeyType, TValueType> Filter(string? expr = null)
    {
        if (string.IsNullOrEmpty(expr))
        {
            return this;
        }
        
        Collection<TKeyType, TValueType> filtered = new();
        foreach ((TKeyType key, MetaDataWrapper<TKeyType, TValueType> value) in this._Container)
        {
            if (value.Value.Contains(expr))
            {
                filtered.Keys.Add(key);
                filtered._Container.Add(key, value);
            }
        }
        return filtered;
    }

    public void Sort(string field)
    {
        Keys.Sort(new VersionedObjectComparer<TKeyType, TValueType>(this._Container, field));
    }

    private void AppendFromJson(JsonNode? node, AbstractUser user)
    {
        JsonObject jsonObject = node!["Value"]!.AsObject();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        foreach ((string key, JsonNode? value) in jsonObject)
        {
            dictionary.Add(key, value!.ToString());
        }

        this.Add(dictionary, user);

        switch (node["Status"]!.GetValue<int>()!)
        {
            case 0:
                this._Container[this.Keys[^1]].Approve();
                break;
            case 2:
                string comment = node["Comment"]?.ToString() ?? string.Empty;
                this._Container[this.Keys[^1]].Reject(comment);
                break;
        }
    }

    public void DumpIntoJson(string filename)
    {
        using StreamWriter fileStream = new(filename);

        JsonArray array = new JsonArray();
        foreach (TKeyType key in this.Keys)
        {
            JsonElement serializedJsonObject = JsonSerializer.SerializeToElement(this._Container[key]);
            array.Add(serializedJsonObject);
        }
        
        string json = JsonSerializer.Serialize(array);
        fileStream.Write(json);
    }
    
    public void LoadFromJson(string filename, Dictionary<string, AbstractUser> users)
    {
        using StreamReader fileStream = new(filename);
        string json = fileStream.ReadToEnd();
        
        JsonArray arr = JsonSerializer.Deserialize<JsonArray>(json)!;

        foreach (JsonNode? node in arr)
        {
            string author = node!["AuthorEmail"]!.ToString();

            AbstractUser user = users[author];
            this.AppendFromJson(node, user);
        }
    }
}