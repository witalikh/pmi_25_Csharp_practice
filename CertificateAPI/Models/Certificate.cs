using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CertificateAPI.Validation;
namespace CertificateAPI;

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

    [JsonIgnore] public string? Owner { get; set; }
}