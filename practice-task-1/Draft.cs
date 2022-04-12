namespace practice_task_1;

public enum DraftStatus: uint
{
    Approved = 0,
    Draft = 1,
    Rejected = 2
}

public class MetaDataWrapper<TKeyType, TObject>
where TObject: class, IGenericValueType<TKeyType>, new()
{
    private TObject _value; // object
    private ErrorsDict _errors; // hidden validation errors
    
    private DraftStatus _status = DraftStatus.Draft;
    private AbstractUser _author;
    
    
    public TKeyType Id => _value.Id;
    public TObject Value => _value;
    public ErrorsDict Errors => _errors;

    
    public MetaDataWrapper(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        this._value = new TObject();
        this._value.Modify(dict);
        
        this._errors = this._value.GetValidationErrors();
        this._author = user;
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