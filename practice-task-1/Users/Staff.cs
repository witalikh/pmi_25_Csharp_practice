namespace practice_task_1;

public class Staff: AbstractUser
{
    public decimal? Salary { get; private set; }
    public DateTime FirstDayInCompany { get; private set; }

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

    public static (Staff, ErrorsDict) Create(Dictionary<string, string> dict, bool secured)
    {
        string firstName = dict["FirstName"];
        string lastName = dict["LastName"];
        
        string email = dict["Email"];
        string password = dict["Password"];

        string salary = dict["Salary"];
        DateTime created = DateTime.Now;
        if (dict.ContainsKey("FirstDayInCompany"))
            DateTime.TryParse(dict["FirstDayInCompany"],out created);
        
        // string role = dict["role"];

        Staff staff = new Staff(
            firstName, lastName, 
            email, password, 
            decimal.Parse(salary), created, secured);

        ErrorsDict errors = staff.GetValidationErrors();
        return (staff, errors);
    }
}