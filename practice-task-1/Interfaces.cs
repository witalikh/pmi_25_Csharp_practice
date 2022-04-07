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

public interface IGenericValueType<TKeyType>: 
    IRecognizable<TKeyType>, 
    IValidatable, 
    IFullyModifiable<IReadOnlyDictionary<string, string>>, 
    ILookupAble<string>
{
}