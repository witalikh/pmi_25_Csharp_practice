using System.Text.Json;
using System.Text.Json.Nodes;

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
            
            JsonArray? staffDictionaries = JsonSerializer.Deserialize<JsonArray>(staffsString);

            if (staffDictionaries != null)
                foreach (JsonNode? dict in staffDictionaries)
                {
                    (Staff admin, ErrorsDict errors) = Staff.Create(dict!.AsObject(), false);
                    
                    Console.WriteLine(admin.Email);
                    Console.WriteLine(admin);
                    
                    this._users.Add(admin.Email!, admin);
                }
        }

        if (!File.Exists("admin.json")) return;
        {
            using StreamReader adminFile = new("admin.json");
            string adminsString = adminFile.ReadToEnd();

            JsonArray? adminDictionaries = JsonSerializer.Deserialize<JsonArray>(adminsString);
            if (adminDictionaries == null) 
                return;
            
            foreach (JsonNode? dict in adminDictionaries)
            {
                Admin admin = Admin.Create(dict!.AsObject(), false).Item1;
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
            this.user = this._users[email];
            this._PrintMessage("AuthenticationSuccess");
        }
        else
        {
            _PrintMessage("AuthenticationFailed");
        }
        
    }

    private void SignUp()
    {
        JsonObject dict = new()
        {
            ["FirstName"] = this._input_field("FirstName"), // TODO
            ["LastName"] = this._input_field("LastName"), // TODO
            ["Email"] = this._input_field("Email"), // TODO
            ["Role"] = "Staff",
            ["Salary"] = "1000"
        };

        string passwordInit = this._input_field("PasswordInit"); // TODO
        string passwordConfirm = this._input_field("PasswordConfirm"); // TODO
        
        dict["Password"] = passwordInit;
        
        
        var (newUser, errors) = Staff.Create(dict, true);

        if (passwordConfirm != passwordInit)
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
            this.user = newUser;
        }
    }

    private void Logout()
    {
        this.user = MainRoles.AnonymousUser;
    }
}