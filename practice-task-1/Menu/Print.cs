namespace practice_task_1;

public partial class Menu<TObject>
{
    private void _printPublicCollection(
        string prefixKey, 
        Collection<string, TObject> collection, 
        Predicate<MetaDataWrapper<string, TObject>> pred,
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
            var objWrapper = collection[key];
            if (!pred(objWrapper)) 
                continue;
            _PrintObject(objWrapper.Value!);
            ++shown;
        }
        _PrintMessage("Size", shown);
    }

    // print menu
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
    
    // print all instances that are approved
    private void PrintPublicAll()
    {
        bool Approved(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Approved;
        _printPublicCollection("PrintAllPrefix", _innerCollection, Approved);
    }

    // print all approved instances with filter
    private void PrintPublicFiltered()
    {
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        var filterResult = _innerCollection.Filter(value);
        bool Approved(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Approved;
        _printPublicCollection("PrintFilteredPrefix", filterResult, Approved, value);
    }
    
    // print all instances that are approved
    private void PrintPrivateAll()
    {
        bool Own(MetaDataWrapper<string, TObject> obj) => obj.Author == this._user;
        _printPublicCollection("PrintAllPrefix", _innerCollection, Own);
    }

    // print all approved instances with filter
    private void PrintPrivateFiltered()
    {
        // TODO: additional filters
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        var filterResult = _innerCollection.Filter(value);
        bool Own(MetaDataWrapper<string, TObject> obj) => obj.Author == this._user;
        _printPublicCollection("PrintFilteredPrefix", filterResult, Own, value);
    }
    
    // print all instances that are approved
    private void PrintDraftAll()
    {
        bool Draft(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Draft;
        _printPublicCollection("PrintAllDraftPrefix", _innerCollection, Draft);
    }

    // print all approved instances with filter
    private void PrintDraftFiltered()
    {
        // TODO: additional filters
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        var filterResult = _innerCollection.Filter(value);
        bool Draft(MetaDataWrapper<string, TObject> obj) => obj.Status == DraftStatus.Draft;
        _printPublicCollection("PrintFilteredDraftPrefix", filterResult, Draft, value);
    }
}