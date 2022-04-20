using System.Text.Json;

namespace practice_task_1;

public partial class Menu<TObject>
{
    private string _input_field(string key)
    {
        Console.Write(this._messages.ContainsKey(key) ? this._messages[key] : key);
        return Console.ReadLine() ?? string.Empty;
    }

    private void LoadUsers()
    {
        if (File.Exists("staff.json"))
        {
            using StreamReader staffFile = new("staff.json");
            string staffsString = staffFile.ReadToEnd();
            
            var staffDictionaries = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(staffsString);
            
            foreach (var dict in staffDictionaries)
            {
                Staff admin = Staff.Create(dict, false).Item1;
                this._users.Add(admin.Email!, admin);
            }
        }

        if (File.Exists("admin.json"))
        {
            using StreamReader adminFile = new("admin.json");
            string adminsString = adminFile.ReadToEnd();

            var adminDictionaries = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(adminsString);

            foreach (var dict in adminDictionaries)
            {
                Admin admin = Admin.Create(dict, false).Item1;
                this._users.Add(admin.Email!, admin);
            }
        }
        
        
    }

    private void DumpUsers()
    {
        using StreamWriter staffFile = new("staff.json");
        // using StreamWriter adminFile = new("admin.json");

        // var admins = new List<Admin>();
        var staff = new List<Staff>();

        foreach ((_, AbstractUser dictUser) in this._users)
        {
            if (dictUser is Staff staffUser)
            {
                staff.Add(staffUser);
            }
            // else if (user is Admin admin)
            // {
            //     admins.Add(admin);
            // }
        }

        // string adminsString = JsonSerializer.Serialize(this._users);
        // adminFile.Write(adminsString);
        
        string staffString = JsonSerializer.Serialize(staff);
        staffFile.Write(staffString);
    }

    private void SignIn()
    {
        string email = this._input_field("Email");
        string password = this._input_field("PasswordLogin"); // TODO

        bool authenticated = this._users.ContainsKey(email) && this._users[email].CheckPassword(password);

        if (authenticated)
        {
            AbstractUser user = this.user;
            this._PrintMessage("AuthenticationSuccess");
        }
        else
        {
            _PrintMessage("AuthenticationFailed");
        }
        
    }

    private void SignUp()
    {
        Dictionary<string, string> dict = new();
        dict["FirstName"] = this._input_field("FirstName"); // TODO
        dict["LastName"] = this._input_field("LastName"); // TODO
        
        dict["Email"] = this._input_field("Email"); // TODO
        
        string password_init = this._input_field("PasswordInit"); // TODO
        string password_confirm = this._input_field("PasswordConfirm"); // TODO

        dict["Role"] = "Staff";
        dict["Password"] = password_init;
        dict["Salary"] = "1000";
        
        var (newUser, errors) = Staff.Create(dict, true);

        if (password_confirm != password_init)
        {
            errors.Add("Password", "PasswordMisMatch");
        }

        if (errors.Count != 0)
        {
            _PrintErrors(ref errors);
        }
        else
        {
            Console.WriteLine(newUser);
            this._users.Add(newUser.Email!, newUser);
        }
    }

    private void Logout()
    {
        this.user = MainRoles.AnonymousUser;
    }
}