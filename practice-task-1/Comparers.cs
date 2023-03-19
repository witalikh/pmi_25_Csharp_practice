namespace practice_task_1;


public class VersionedObjectComparer<TKeyType, TObject> : IComparer<TKeyType>
where TObject: class, IGenericValueType<TKeyType>, new()
{
    private Dictionary<TKeyType, MetaDataWrapper<TKeyType, TObject>> _container;
    private string field;
        
    public VersionedObjectComparer(
        Dictionary<TKeyType, MetaDataWrapper<TKeyType, TObject>> container, 
        string field)
    {
        this._container = container;
        this.field = field;
    }
        
    public int Compare(TKeyType? x, TKeyType? y)
    {
        switch (x)
        {
            case null when y == null:
                return 0;
            case null:
                return -1;
            case not null when y == null:
                return 1;
            case not null:
                break;
        }

        TObject objectFirst = this._container[x].Value;
        TObject objectSecond = this._container[y].Value;

        IComparable valueFirst = objectFirst.GetFieldValueByName(field);
        IComparable valueSecond = objectSecond.GetFieldValueByName(field);

        return valueFirst.CompareTo(valueSecond);

    }
}