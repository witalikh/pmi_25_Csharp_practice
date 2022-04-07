namespace practice_task_1;

public enum DraftStatus: uint
{
    Approved = 0,
    Draft = 1,
    Rejected = 2;
}

public class MetaDataWrapper<TKeyType, TValueType>
where TValueType: class, IGenericValueType<TKeyType>, new()
{
    private TValueType _value;
    private ErrorsDict _errors;
    
    private DraftStatus _status = DraftStatus.Draft;

    private AbstractUser _user;
    
    public TKeyType? Id => _status == DraftStatus.Rejected ? default : _value.Id;
    
    public ErrorsDict Errors => _errors;

    
    public MetaDataWrapper(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        this._value = new TValueType();
        this._value.Modify(dict);
        
        this._errors = this._value.GetValidationErrors();
        this._user = user;
    }

    public void Publish()
    {
        this._status = DraftStatus.Approved;
    }

    public void Discard()
    {
        this._status = DraftStatus.Rejected;
    }
    
    
}