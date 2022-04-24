namespace practice_task_1;

public partial class Menu<TObject>
{
    private void Approve()
    {
        if (!this._user.HasApprovePerms)
        {
            _PrintMessage("PermissionDenied");
            return;
        }
        
        _PrintMessage("EnterId");
        string key = Console.ReadLine() ?? string.Empty;

        if (!_innerCollection.Contains(key))
        {
            _PrintMessage("IdAbsent");
        }
        else if (_innerCollection[key].Status != DraftStatus.Draft)
        {
            _PrintMessage("NotInDraft");
        }
        else
        {
            this._innerCollection[key].Approve();
            _PrintMessage("SuccessApprove");
        }
    }

    private void Reject()
    {
        if (!this._user.HasApprovePerms)
        {
            _PrintMessage("PermissionDenied");
            return;
        }
        
        _PrintMessage("EnterId");
        string key = Console.ReadLine() ?? string.Empty;

        if (!_innerCollection.Contains(key))
        {
            _PrintMessage("IdAbsent");
        }
        else if (_innerCollection[key].Status != DraftStatus.Draft)
        {
            _PrintMessage("NotInDraft");
        }
        else
        {
            _PrintMessage("RejectComment");
            string comment = Console.ReadLine() ?? string.Empty;
            
            this._innerCollection[key].Reject(comment);
            _PrintMessage("SuccessReject");
        }
    }
}