namespace practice_task_1;


public class VersionedObjectComparer<TKeyType, TObject> : IComparer<TKeyType>
where TObject: class, IGenericValueType<TKeyType>, new()
{
    private Dictionary<TKeyType, VersionedObject<TKeyType, TObject>> _container;
    private string field;
        
    public VersionedObjectComparer(
        Dictionary<TKeyType, VersionedObject<TKeyType, TObject>> container, 
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

        TObject? objectFirst = this._container[x].Value;
        TObject? objectSecond = this._container[y].Value;
            
        switch (objectFirst)
        {
            case null when objectSecond == null:
                return 0;
            case null:
                return -1;
            case not null when objectSecond == null:
                return 1;
            case not null:
                break;
        }
            
        IComparable valueFirst = objectFirst.GetFieldValueByName(field);
        IComparable valueSecond = objectSecond.GetFieldValueByName(field);

        return valueFirst.CompareTo(valueSecond);

    }
}