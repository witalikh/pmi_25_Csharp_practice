namespace practice_task_3;

public class AbstractUser
{
    private string _firstName;
    private string _lastName;

    private string _email;
    private string _password;

    private Role _role;
    
    private string Password
    {
        set => _password = value;
    }
}