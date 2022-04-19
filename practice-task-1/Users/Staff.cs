namespace practice_task_1;

public class Staff: AbstractUser
{
    private decimal _salary;
    private DateTime _firstDayInCompany;

    public Staff(string firstName, string lastName, string email, string password, decimal salary)
    {
        this._role = MainRoles.Staff;

        this._firstName = firstName;
        this._lastName = lastName;

        this._email = email;
        this.SetPassword(password);
        
        this._firstDayInCompany = DateTime.Now;
        this._salary = salary;
    }
}