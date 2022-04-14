namespace practice_task_1;

public class Role
{
    public bool IsSuperAdmin { get; } = false;
    public bool HasApprovalPerms { get; } = false;
    public bool HasEditPermissions { get; } = false;
}

public abstract class AbstractUser
{
    private string _firstName;
    private string _lastName;

    private string _email;
    private string _password;

    private Role _role;

    public bool IsSuperAdmin => this._role.IsSuperAdmin;
    public bool HasApprovePerms => this._role.IsSuperAdmin || this._role.HasApprovalPerms;
    public bool HasEditPerms => this._role.IsSuperAdmin || this._role.HasEditPermissions;
}