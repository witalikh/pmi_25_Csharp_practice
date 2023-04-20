using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CertificateAPI;

public static class Settings
{
    public const string NamePattern = "^[\\w'\\-,.][^0-9_!¡?÷?¿/\\\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$";
    public const string PassportPattern = "^[a-zA-Z]{2}[0-9]{6}$";

    public static readonly string[] Vaccines =
    {
        "Pfizer", "CoronaVac", "Moderna", "AstraZeneca", "Jannsen"
    };

    public const int MinAge = 14;
    public const int MaxAge = 125;

    public const int MaxCertDelay = 14;
}

public class VaccineValidator : ValidationAttribute
{      
    protected override ValidationResult 
        IsValid(object? value, ValidationContext validationContext)
    {   
        string vaccine = Convert.ToString(value);
        if (Settings.Vaccines.Contains(vaccine))
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult
                ("Invalid vaccine.");
        }
    }        
}

public class StartDateValidator : ValidationAttribute
{      
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        Certificate cert = validationContext.ObjectInstance as Certificate;
        DateTime startDate = Convert.ToDateTime(value);
        if (cert.EndDate != null && startDate <= cert.EndDate)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult
                ("Start date cannot be later than end date");
        }
    }        
}

public class BirthDateValidator : ValidationAttribute
{      
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        Certificate cert = validationContext.ObjectInstance as Certificate;
        DateTime birthDate = Convert.ToDateTime(value);
        if (cert.StartDate != null && birthDate <= cert.StartDate)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult
                ("Birth date cannot be later than start date");
        }
    }        
}

public class Certificate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default(int);
    
    [Required]
    [RegularExpression(
        Settings.NamePattern,
        ErrorMessage = "Name can contain only letters and hyphens and should be at least with two words."
        )
    ]
    public string? UserName { get; set; }
    
    [Required]
    [RegularExpression(
        Settings.PassportPattern, 
        ErrorMessage="Passport code should be in format XXYYYYYY where X - letters, Y-digits.")
    ]
    public string? Passport { get; set; }
    
    [Required]
    [StartDateValidator]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    [BirthDateValidator]
    public DateTime BirthDate { get; set; }

    [Required]
    [VaccineValidator]
    public string Vaccine
    {
        get;
        set;
    }
}