using System.Globalization;

namespace practice_task_1;

public partial class Menu<TObject>
{
    private void _PrintField(string key, string? value)
    {
        Console.Write(this._messages.ContainsKey(key) ? this._messages[key] : key);
        Console.WriteLine(value);
    }
    
    private void _PrintUser(AbstractUser user)
    {
        Console.WriteLine("---");
        
        _PrintField("FirstName", user.FirstName);
        _PrintField("LastName", user.LastName);
        _PrintField("Email", user.Email);

        if (user is Staff staff)
        {
            _PrintField("Salary", staff.Salary.ToString());
            _PrintField("FirstDayInCompany", staff.FirstDayInCompany.ToString(CultureInfo.InvariantCulture));
        }
        Console.WriteLine("+++");
    }
    
    private void PrintAllUsers()
    {
        _PrintMessage("PrintUsers", this._users.Count);
        foreach( (_, AbstractUser user) in this._users)
        {
            _PrintUser(user);
        }
    }
    
    private void _printPublicCollection(
        string prefixKey, 
        Collection<string, TObject> collection, 
        Predicate<MetaDataWrapper<string, TObject>> pred,
        bool withMeta = false,
        string? value = null )
    {
        if (value == null)
        {
            _PrintMessage(prefixKey);
        }
        else
        {
            _PrintMessage(prefixKey, value);
        }

        uint shown = 0; 
        foreach(string key in collection.Keys)
        {
            MetaDataWrapper<string, TObject> objWrapper = collection[key];
            if (!pred(objWrapper)) 
                continue;
            _PrintObject(objWrapper, withMeta);
            ++shown;
        }
        _PrintMessage("Size", shown);
    }
    
    private void PrintMenu()
    {
        switch (this._user)
        {
            case Staff:
                _PrintMessage("StaffMenu");
                break;
            case Admin:
                _PrintMessage("AdminMenu");
                break;
            default:
                _PrintMessage("AnonymousMenu");
                break;
        }
    }
    
    private void PrintPublicAll()
    {
        bool Approved(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Approved;
        _printPublicCollection("PrintAllPrefix", _innerCollection, Approved);
    }
    
    private void PrintPublicFiltered()
    {
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        Collection<string, TObject> filterResult = _innerCollection.Filter(value);
        bool Approved(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Approved;
        _printPublicCollection("PrintFilteredPrefix", filterResult, Approved, false, value);
    }
    
    private void PrintPrivateAll()
    {
        _PrintMessage("ChooseType");
        string option = Console.ReadLine()!.Trim().ToLower();

        bool Condition(MetaDataWrapper<string, TObject> obj)
        {
            return option switch
            {
                "0" => obj.Status == DraftStatus.Approved && obj.Author == this._user,
                "1" => obj.Status == DraftStatus.Draft && obj.Author == this._user,
                "2" => obj.Status == DraftStatus.Rejected && obj.Author == this._user,
                _ => obj.Author == this._user
            };
        }
        _printPublicCollection("PrintAllPrefix", _innerCollection, Condition, true);
    }
    
    private void PrintPrivateFiltered()
    {
        _PrintMessage("ChooseType");
        string option = Console.ReadLine()!.Trim().ToLower();

        bool Condition(MetaDataWrapper<string, TObject> obj)
        {
            return option switch
            {
                "0" => obj.Status == DraftStatus.Approved && obj.Author == this._user,
                "1" => obj.Status == DraftStatus.Draft && obj.Author == this._user,
                "2" => obj.Status == DraftStatus.Rejected && obj.Author == this._user,
                _ => obj.Status == DraftStatus.Approved && obj.Author == this._user
            };
        }
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        Collection<string, TObject> filterResult = _innerCollection.Filter(value);
        _printPublicCollection("PrintFilteredPrefix", filterResult, Condition, true,value);
    }
    
    private void PrintDraftAll()
    {
        bool Draft(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Draft;
        _printPublicCollection("PrintAllDraftPrefix", _innerCollection, Draft, true);
    }
    
    private void PrintDraftFiltered()
    {
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        Collection<string, TObject> filterResult = _innerCollection.Filter(value);
        bool Draft(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Draft;
        _printPublicCollection("PrintFilteredDraftPrefix", filterResult, Draft, true, value);
    }
}