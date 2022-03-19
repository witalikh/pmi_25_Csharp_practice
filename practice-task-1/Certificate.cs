﻿namespace practice_task_1;

public class Certificate:
    IRecognizable<string>, 
    IValidatable, 
    IFullyModifiable<IReadOnlyDictionary<string, string>>, 
    ILookupAble<string>
{
    // internal field declarations
    private readonly Dictionary<string, object?> _itemsDict = new();

    // property declarations
    public string? Id
    {
        get => _itemsDict["Id"] as string;
        private set => _itemsDict["Id"] = value;
    }

    public string? Username
    {
        get => _itemsDict["Username"] as string;
        private set => _itemsDict["Username"] = value;
    }

    public string? Passport
    {
        get => _itemsDict["Passport"] as string;
        private set => _itemsDict["Passport"] = value;
    }

    public DateTime? StartDate
    {
        get => _itemsDict["StartDate"] as DateTime?;
        private set => _itemsDict["StartDate"] = value;
    }

    public DateTime? EndDate
    {
        get => _itemsDict["EndDate"] as DateTime?;
        private set => _itemsDict["EndDate"] = value;
    }

    public DateTime? BirthDate
    {
        get => _itemsDict["BirthDate"] as DateTime?;
        private set => _itemsDict["BirthDate"] = value;
    }
    public string? Vaccine
    {
        get => _itemsDict["Vaccine"] as string;
        private set => _itemsDict["Vaccine"] = value;
    }

    public static string[] Keys()
    {
        string[] keys = {"Id", "Username", "Passport", "StartDate", "EndDate", "BirthDate", "Vaccine"};
        return keys;
    }

    public Dictionary<string, string> Items()
    {
        Dictionary<string, string> itemsDict = new();
        foreach ((string key, object? value) in _itemsDict)
        {
            if (value is null)
            {
                itemsDict[key] = string.Empty;
            }
            itemsDict[key] = value?.ToString() ?? string.Empty ;
        }

        return itemsDict;
    }

    public Certificate(){}
    public Certificate(IReadOnlyDictionary<string, string> valuesDict)
    {
        this.Modify(valuesDict);
    }
    public void Modify(IReadOnlyDictionary<string, string> valuesDict)
    {
        Id = valuesDict["Id"];
        
        Username = ValidationUtils.validate_regex(valuesDict["Username"], Settings.NamePattern);
        Passport = ValidationUtils.validate_regex(valuesDict["Passport"], Settings.PassportPattern);
        
        StartDate = ValidationUtils.try_to_datetime(valuesDict["StartDate"]);
        EndDate = ValidationUtils.try_to_datetime(valuesDict["EndDate"]);
        BirthDate = ValidationUtils.try_to_datetime(valuesDict["BirthDate"]);
        
        Vaccine = ValidationUtils.validate_choice(valuesDict["Vaccine"], Settings.Vaccines);
    }

    private void _GetSequenceErrors(ref ErrorsDict errorsDict)
    {
        if (StartDate != null && EndDate != null && StartDate >= EndDate)
            errorsDict.Add("EndDate", "StartDateEndDateSequenceError");

        if (BirthDate != null && StartDate != null && BirthDate >= StartDate)
            errorsDict.Add("BirthDate", "BirthDateStartDateSequenceError");

        if (BirthDate != null && EndDate != null && BirthDate >= EndDate)
            errorsDict.Add("BirthDate", "BirthDateEndDateSequenceError");
    }
    
    private void _GetLegislationErrors(ref ErrorsDict errorsDict)
    {
        DateTime todayMinusMinAge = DateTime.Today.AddYears(-Settings.MinAge);
        DateTime todayMinusMaxAge = DateTime.Today.AddYears(-Settings.MaxAge);

        DateTime todayPlusMaxCert = DateTime.Today.AddDays(Settings.MaxCertDelay);
        DateTime? birthdayPlusMinAge = BirthDate?.AddYears(Settings.MinAge);
        
        if (BirthDate != null && BirthDate > todayMinusMinAge )
            errorsDict.Add("BirthDate", "BirthDateTooEarly");

        if (BirthDate != null && BirthDate <= todayMinusMaxAge)
            errorsDict.Add("BirthDate", "BirthDateTooLate");

        if (StartDate != null && StartDate >= todayPlusMaxCert)
            errorsDict.Add("StartDate", "StartDateTooLate");
        
        if (StartDate != null && birthdayPlusMinAge != null && StartDate <= birthdayPlusMinAge)
            errorsDict.Add("StartDate", "StartDateTooEarly");
    }
    
    public ErrorsDict GetValidationErrors()
    {
        var errorsDict = new ErrorsDict();
        foreach ((string property, object? value) in _itemsDict)
        {
            if (value == null)
            {
                errorsDict.Add(property, $"{property}FormatError");
            }
        }

        _GetSequenceErrors(ref errorsDict);
        _GetLegislationErrors(ref errorsDict);

        return errorsDict;
    }

    public bool Contains(string expr)
    {
        var values = _itemsDict.Values;
        foreach (object? value in values)
        {
            if (value != null)
            {
                string strValue = value.ToString() ?? string.Empty;
                if (strValue.ToLower().Contains(expr.ToLower()))
                    return true;
            }
        }
        return false;
    }
}