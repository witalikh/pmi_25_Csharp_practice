using System.ComponentModel.DataAnnotations;
using System.Globalization;

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

public class Certificate
{
    public int Id { get; set; }
    
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
    [Compare("EndDate", ErrorMessage = "Start date cannot be later than end date")]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    [Compare("StartDate", ErrorMessage = "Birth date cannot be later than start date")]
    public DateTime BirthDate { get; set; }

    public string? Vaccine { get; set; }

    public override string ToString()
    {
        return string.Concat(
            Passport,
            UserName,
            Vaccine,
            BirthDate.ToString(CultureInfo.CurrentCulture),
            StartDate.ToString(CultureInfo.CurrentCulture),
            EndDate.ToString(CultureInfo.CurrentCulture),
            Id.ToString()
        );
    }
}