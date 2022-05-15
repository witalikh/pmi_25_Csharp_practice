using System.ComponentModel.DataAnnotations;
using CertificateAPI.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CertificateAPI;

// [Keyless]
// public class UserCertificate
// {
//     public int UserId;
//     public User User;
//     public int CertificateId;
//     public Certificate Certificate;
// }

public class User : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    
    [PersonalData]
    public string LastName { get; set; }
    
}

public class Register
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    [RegularExpression(
        Settings.EmailPattern, 
        ErrorMessage="Email should match a pattern, this doesn't")
    ]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class Login
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

