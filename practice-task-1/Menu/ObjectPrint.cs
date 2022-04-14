namespace practice_task_1;

public partial class Menu<TObject>
{
    private void _printCollection(string prefixKey, Collection<string, TObject> collection)
    {
        _PrintMessage(prefixKey);
        
        uint shown = 0; 
        foreach(string key in collection.Keys)
        {
            var objWrapper = collection[key];
            if (objWrapper.Status == DraftStatus.Approved)
            {
                _PrintObject(objWrapper.Value!);
                ++shown;
            }
        }
        _PrintMessage("Size", shown);
    }
    
    private void PrintAll()
    {
        _printCollection("PrintAllPrefix", _innerCollection);
    }

    private void PrintFiltered()
    {
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        var filterResult = _innerCollection.Filter(value);
        _printCollection("PrintFilteredPrefix", filterResult);
    }
}