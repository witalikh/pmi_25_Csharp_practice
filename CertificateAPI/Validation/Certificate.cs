using System.ComponentModel.DataAnnotations;

namespace CertificateAPI.Validation;

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