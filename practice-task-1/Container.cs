using System.Text.Json;

namespace practice_task_1;

public class Collection
{
    // step off the original sequential data structure for sake of speed
    private readonly SortedSet<string> _keys;
    private List<Certificate> _container;

    public int Size => _container.Count;

    public Collection()
    {
        _keys = new SortedSet<string>();
        _container = new List<Certificate>();
    }
    
    public Certificate this[int i] => _container[i];

    public bool Add(Certificate cert)
    {
        if ((cert.Id == null) || (_keys.Contains(cert.Id)))
            return false;

        _keys.Add(cert.Id);
        _container.Add(cert);
        return true;
    }

    public bool Remove(string id)
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

    public bool Contains(string id) => _keys.Contains(id);

    public int? GetIndex(string id)
    {
        if (!Contains(id))
            return null;
        
        for (int i = 0; i < Size; ++i)
        {
            if (_container[i].Id == id)
            {
                return i;
            }
        }

        return null;
    }

    public bool Edit(string id, Certificate newCert)
    {
        if (newCert.Id != id) 
            return false;
        
        int? index = GetIndex(id);

        if (index != null)
        {
            _container[index.Value] = newCert;
            return true;
        }

        return false;
    }

    public Collection Filter(string? expr = null)
    {
        if (string.IsNullOrEmpty(expr))
        {
            return this;
        }
        
        Collection filtered = new Collection();
        foreach (Certificate certificate in _container)
        {
            if (!certificate.Contains(expr) || certificate.Id == null)
                continue;
            
            filtered._keys.Add(certificate.Id);
            filtered._container.Add(certificate);
        }

        return filtered;
    }

    public void Sort(string field)
    {
        _container = _container.OrderBy(
            certificate => certificate.GetType().GetProperty(field)?.GetValue(certificate)
            ).ToList();
    }

    public void DumpIntoJson(string filename)
    {
        using var fileStream = new StreamWriter(filename);
        string json = JsonSerializer.Serialize(_container);
        fileStream.Write(json);
    }
    

    public Collection LoadFromJson(string filename)
    {
        Collection errorCollection = new Collection();
        
        using StreamReader fileStream = new StreamReader(filename);
        string json = fileStream.ReadToEnd();
        
        var arr = JsonSerializer.Deserialize<List<Certificate>>(json);

        if (arr == null)
            return errorCollection;
        
        foreach (Certificate certificate in arr)
        {
            var errors = certificate.GetValidationErrors();
            if (certificate.Id != null && errors.Count == 0 && !Contains(certificate.Id))
            {
                Add(certificate);
            }
            else
            {
                errorCollection.Add(certificate);
            }
        }
        
        return errorCollection;
    }
}