using System.Text.Json.Nodes;

namespace practice_task_1;

public class Staff: AbstractUser
{
    public decimal? Salary { get; private init; }
    public DateTime FirstDayInCompany { get; private init; }

    public Staff(
        string firstName, 
        string lastName, 
        string email, 
        string password, 
        decimal salary,
        DateTime? created = null,
        bool secured = true)
    {
        this.Role = MainRoles.Staff;
        this.FirstDayInCompany = created ?? DateTime.Now;

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
        
        this.FirstDayInCompany = DateTime.Now;
        this.Salary = salary;
    }

    public override ErrorsDict GetValidationErrors()
    {
        ErrorsDict errors = base.GetValidationErrors();
        
        if (Salary == null)
            errors.Add("Salary", "SalaryFormat");

        return errors;
    }

    public static (Staff, ErrorsDict) Create(JsonObject dict, bool secured)
    {
        string firstName = dict[nameof(FirstName)]?.GetValue<string>() ?? string.Empty;
        string lastName = dict[nameof(LastName)]?.GetValue<string>() ?? string.Empty;
        
        string email = dict[nameof(Email)]?.GetValue<string>() ?? string.Empty;
        string password = dict[nameof(Password)]?.GetValue<string>() ?? string.Empty;

        decimal salary = dict[nameof(Salary)]?.GetValue<decimal>() ?? decimal.Zero;
        DateTime created = dict[nameof(FirstDayInCompany)]?.GetValue<DateTime>() ?? DateTime.Now;

        Staff staff = new Staff(
            firstName, lastName, 
            email, password, 
            salary, created, secured);

        ErrorsDict errors = staff.GetValidationErrors();
        return (staff, errors);
    }
}