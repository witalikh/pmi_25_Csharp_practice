namespace practice_task_1;

public partial class Menu<TObject>
{
    private void Add()
    {
        if (!this.user.HasEditPerms)
        {
            _PrintMessage("PermissionDenied");
            return;
        }
        
        string[] keys = _emptySample.Keys();
        var values = new Dictionary<string, string>();

        Console.WriteLine("+++");
        foreach (string property in keys)
        {
            Console.Write(_messages.ContainsKey(property) ? _messages[property] : property + ": ");
            values[property] = (Console.ReadLine() ?? string.Empty).Trim();
        }
        Console.WriteLine("---");

        ErrorsDict errors = _innerCollection.Add(values, this.user);
        
        if (errors.Count != 0)
            _PrintErrors(ref errors);
        else
            _PrintMessage("SuccessAdd");
    }

    private void Edit()
    {
        if (!this.user.HasEditPerms)
        {
            _PrintMessage("PermissionDenied");
            return;
        }
        
        _PrintMessage("EnterId");
        string key = Console.ReadLine() ?? string.Empty;

        if (!_innerCollection.Contains(key))
        {
            _PrintMessage("IdAbsent");
        }
        else
        {
            Dictionary<string, string> oldData = this._innerCollection[key].Value!.FancyItems();
            
            string? field = _ChooseField();
            if (field == null)
            {
                _PrintMessage("WrongField");
                return;
            }
            
            Console.Write(_messages.ContainsKey(field) ? _messages[field] : field + ": ");
            oldData[field] = Console.ReadLine() ?? string.Empty;
            
            ErrorsDict errors = _innerCollection.Edit(key, oldData, this.user);
            
            if (errors.Count != 0)
                _PrintErrors(ref errors);
            else
                _PrintMessage("SuccessEdit");
        }
    }

    private void Delete()
    {
        if (!this.user.HasEditPerms)
        {
            _PrintMessage("PermissionDenied");
            return;
        }
        
        // scan id
        _PrintMessage("EnterId");
        string id = Console.ReadLine() ?? string.Empty;

        // try to delete by id
        _PrintMessage(!_innerCollection.Delete(id) ?"IdAbsent" : "SuccessDelete");
    }
}