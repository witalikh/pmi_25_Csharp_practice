namespace practice_task_1;

public abstract class AbstractUser
{
    protected string _firstName;
    protected string _lastName;

    protected string _email;
    protected string _password;

    protected Role _role;

    public bool IsSuperAdmin => this._role.IsSuperAdmin;
    public bool HasApprovePerms => this._role.IsSuperAdmin || this._role.HasApprovalPerms;
    public bool HasEditPermsForOtherInstances => 
        this._role.IsSuperAdmin || this._role.HasEditPermissionsForOtherInstances;
    public bool HasEditPerms => this._role.IsSuperAdmin || this._role.HasEditPermissions;

    public void SetPassword(string password)
    {
        this._password = password.GetHashCode().ToString();
    }
    
    public bool CheckPassword(string password)
    {
        return this._password == password.GetHashCode().ToString();
    }
    
    
}