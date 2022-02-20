namespace practice_task_1;
using StringListDict = Dictionary<string, LinkedList<string>>;
public class Certificate
{
    // internal field declarations
    private readonly string? _username;
    private readonly string? _passport;
    private readonly string? _vaccine;

    // interface field declarations
    public string? Id { get; init; }

    public string? Username
    {
        get => _username;
        init => _username = ValidationUtils.validate_regex(value, Settings.NamePattern);
    }

    public string? Passport
    {
        get => _passport;
        init => _passport = ValidationUtils.validate_regex(value, Settings.PassportPattern);
    }
    public DateTime? StartDate { get; init; } 
    public DateTime? EndDate { get; init; }
    public DateTime? BirthDate { get; init; }
    public string? Vaccine
    {
        get => _vaccine;
        init => _vaccine = ValidationUtils.validate_choice(value, Settings.Vaccines);
    }

    public static string[] Keys()
    {
        string[] keys = {"Id", "Username", "Passport", "StartDate", "EndDate", "BirthDate", "Vaccine"};
        return keys;
    }

    public string[] Values()
    {
        string[] keys = Keys();
        string[] values = new string[keys.Length];

        for (int i = 0; i < keys.Length; i++)
        {
            object? obj = GetType().GetProperty(keys[i])?.GetValue(this);
            values[i] = obj?.ToString() ?? string.Empty;
        }

        return values;
    }
    
    public Certificate(){}
    
    // hardcoded for functionality
    // field order is program convention
    public Certificate(params string[] args): 
        this(
            id: args[0], 
            username: args[1], 
            passport: args[2], 
            startDate: args[3], 
            endDate: args[4], 
            birthDate: args[5], 
            vaccine: args[6])
    {}

    private Certificate(
        string id,
        string username, 
        string passport, 
        string startDate, 
        string endDate, 
        string birthDate, 
        string vaccine)
    {
        Id = id;
        
        Username = username;
        Passport = passport;
        
        StartDate = ValidationUtils.try_to_datetime(startDate);
        EndDate = ValidationUtils.try_to_datetime(endDate);
        BirthDate = ValidationUtils.try_to_datetime(birthDate);
        
        Vaccine = vaccine;
    }
    
    private void _AddError(ref StringListDict dict, string key, string value)
    {
        if (dict.ContainsKey(key) == false)
            dict[key] = new LinkedList<string>();
        
        dict[key].AddLast(value);
    }
    
    private void _GetSequenceErrors(ref StringListDict dict)
    {
        if (StartDate != null && EndDate != null && StartDate >= EndDate)
            _AddError(ref dict, "EndDate", "StartDateEndDateSequenceError");

        if (BirthDate != null && StartDate != null && BirthDate >= StartDate)
            _AddError(ref dict, "BirthDate", "BirthDateStartDateSequenceError");

        if (BirthDate != null && EndDate != null && BirthDate >= EndDate)
            _AddError(ref dict, "BirthDate", "BirthDateEndDateSequenceError");
    }
    
    private void _GetLegislationErrors(ref StringListDict dict)
    {
        DateTime todayMinusMinAge = DateTime.Today.AddYears(-Settings.MinAge);
        DateTime todayMinusMaxAge = DateTime.Today.AddYears(-Settings.MaxAge);

        DateTime todayPlusMaxCert = DateTime.Today.AddDays(Settings.MaxCertDelay);
        DateTime? birthdayPlusMinAge = BirthDate?.AddYears(Settings.MinAge);
        
        if (BirthDate != null && BirthDate > todayMinusMinAge )
            _AddError(ref dict, "BirthDate", "BirthDateTooEarly");

        if (BirthDate != null && BirthDate <= todayMinusMaxAge)
            _AddError(ref dict, "BirthDate", "BirthDateTooLate");

        if (StartDate != null && StartDate >= todayPlusMaxCert)
            _AddError(ref dict, "StartDate", "StartDateTooLate");
        
        if (StartDate != null && birthdayPlusMinAge != null && StartDate <= birthdayPlusMinAge)
            _AddError(ref dict, "StartDate", "StartDateTooEarly");
    }
    
    public StringListDict GetValidationErrors()
    {
        var errorsDict = new StringListDict();

        var properties = GetType().GetProperties();
        foreach (var property in properties)
        {
            if (property.GetValue(this) == null)
            {
                _AddError(ref errorsDict, property.Name, $"{property.Name}FormatError");
            }
        }

        _GetSequenceErrors(ref errorsDict);
        _GetLegislationErrors(ref errorsDict);

        return errorsDict;
    }

    public bool Contains(string expr)
    {
        string[] values = Values();
        foreach (string value in values)
        {
            if (value.ToLower().Contains(expr.ToLower()))
                return true;
        }
        return false;
    }
}