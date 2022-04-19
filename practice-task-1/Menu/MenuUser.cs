namespace practice_task_1;

public partial class Menu<TObject>
{
    private string _input_password(string key)
    {
        Console.Write((this._messages[key]));
        string x = Console.ReadLine() ?? string.Empty;
        return x.GetHashCode().ToString();
    }

    private string _input_field(string key)
    {
        Console.Write((this._messages[key]));
        return Console.ReadLine() ?? string.Empty;
    }

    private void LoadUsers()
    {
        
    }

    private void DumpUsers()
    {
        
    }
    
    public void SignIn()
    {
        string email = this._input_field("Email");
        string password = this._input_password("PasswordLogin"); // TODO

        AbstractUser user = this.user;


    }
    
    public void SignUp()
    {
        string firstName = this._input_field("FirstName"); // TODO
        string lastName = this._input_field("LastName"); // TODO
        
        string email = this._input_field("Email"); // TODO
        
        string password_init = this._input_password("PasswordInit"); // TODO
        string password_confirm = this._input_password("PasswordConfirm"); // TODO

        Role role = MainRoles.Staff;

        if (password_confirm != password_init)
        {
            
        }

        Staff newUser = new Staff(firstName, lastName, email, password_init, 1000);
        
        
        this._users.Add(email, newUser);
        this.user = newUser;
    }

    public void Logout()
    {
        this.user = MainRoles.AnonymousUser;
    }
}