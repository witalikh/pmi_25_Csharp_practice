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
    
    public static (Admin, ErrorsDict) Create(Dictionary<string, string> dict, bool secured)
    {
        string firstName = dict["FirstName"];
        string lastName = dict["LastName"];
        
        string email = dict["Email"];
        string password = dict["Password"];
        
        // string role = dict["role"];

        Admin admin = new Admin(firstName, lastName, email, password, secured);
        ErrorsDict errors = admin.GetValidationErrors();

        return (admin, errors);
    }
}