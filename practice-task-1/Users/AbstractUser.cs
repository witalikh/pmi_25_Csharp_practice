using System.Text.Json.Serialization;
using static practice_task_1.ValidationUtils;
using static practice_task_1.Hashers;
namespace practice_task_1;

public abstract class AbstractUser
{
    private string? _firstName;
    private string? _lastName;

    private string? _email;
    private string? _password;
    
    
    public string? FirstName
    {
        get => _firstName;
        protected set => _firstName = validate_regex(value, "[a-zA-Z-]")!;
    }

    public string? LastName
    {
        get => _lastName;
        protected set => _lastName = validate_regex(value, "[a-zA-Z-]")!;
    }

    public string? Email
    {
        get => _email;
        protected set => _email = validate_regex(value, "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+")!;
    }

    public string? Password
    {
        get => _password; 
        protected set => _password = ComputeSha256Hash(value);
    }

    [JsonIgnore]
    public Role Role
    {
        get;
        protected set;
    }

    protected void SetUnsecuredPassword(string password)
    {
        _password = password;
    }

    public virtual ErrorsDict GetValidationErrors()
    {
        ErrorsDict errors = new();
        if (this._firstName == null)
        {
            errors.Add("FirstName", "FirstNameFormat");
        }
        
        if (this._lastName == null)
        {
            errors.Add("LastName", "LastNameFormat");
        }
        
        if (this._email == null)
        {
            errors.Add("Email", "EmailFormat");
        }
        
        if (this._password == null)
        {
            errors.Add("Password", "PasswordFormat");
        }

        return errors;
    }

    // bool
    [JsonIgnore] public bool IsSuperAdmin => Role.IsSuperAdmin;
    [JsonIgnore] public bool HasApprovePerms => Role.IsSuperAdmin || Role.HasApprovalPerms;
    [JsonIgnore] public bool HasEditPermsForOtherInstances => 
        Role.IsSuperAdmin || Role.HasEditPermissionsForOtherInstances;
    [JsonIgnore] public bool HasEditPerms => Role.IsSuperAdmin || Role.HasEditPermissions;

    public bool CheckPassword(string password)
    {
        return Password == ComputeSha256Hash(password);
    }
}