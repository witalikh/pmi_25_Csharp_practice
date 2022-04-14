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
    public TKeyType Id => Value.Id;
    public TObject Value { get; }
    public ErrorsDict Errors { get; }
    
    public DraftStatus Status { get; private set; } = DraftStatus.Draft;
    public AbstractUser Author { get; }

    public MetaDataWrapper(IReadOnlyDictionary<string, string> dict, AbstractUser user)
    {
        this.Value = new TObject();
        this.Value.Modify(dict);
        
        this.Errors = this.Value.GetValidationErrors();
        this.Author = user;
    }

    public void Publish()
    {
        this.Status = DraftStatus.Approved;
    }

    public void Discard()
    {
        this.Status = DraftStatus.Rejected;
    }
}