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

public interface ILookupAble<TInnerObject>
{
    public bool Contains(string lookupExpr);
    public string[] Keys();

    public TInnerObject GetFieldValueByName(string field);
    
    public Dictionary<string, string> FancyItems();
}

public interface IGenericValueType<out TKeyType>: 
    IRecognizable<TKeyType>, 
    IValidatable, 
    IFullyModifiable<IReadOnlyDictionary<string, string>>, 
    ILookupAble<IComparable>
{
}