using System.Text.Json.Nodes;

namespace practice_task_1;

public class Admin: AbstractUser
{
    
    public Admin(
        string firstName, 
        string lastName, 
        string email, 
        string password,
        bool secured = true)
    {
        this.Role = MainRoles.Admin;

        this.FirstName = firstName;
        this.LastName = lastName;

        this.Email = email;
        if (secured)
        {
            this.Password = password;
        }
        else
        {
            this.SetUnsecuredPassword(password);
        }
    }
    
    public static (Admin, ErrorsDict) Create(JsonObject dict, bool secured)
    {
        string firstName = dict[nameof(FirstName)]?.GetValue<string>() ?? string.Empty;
        string lastName = dict[nameof(LastName)]?.GetValue<string>() ?? string.Empty;
        
        string email = dict[nameof(Email)]?.GetValue<string>() ?? string.Empty;
        string password = dict[nameof(Password)]?.GetValue<string>() ?? string.Empty;

        Admin admin = new Admin(firstName, lastName, email, password, secured);
        ErrorsDict errors = admin.GetValidationErrors();

        return (admin, errors);
    }
}