namespace CertificateAPI.Validation;

public static class Settings
{
    public const string NamePattern = @"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$";
    public const string PassportPattern = @"^[a-zA-Z]{2}[0-9]{6}$";
    public const string EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";

    public static readonly string[] Vaccines =
    {
        "Pfizer", "CoronaVac", "Moderna", "AstraZeneca", "Jannsen"
    };

    public const int MinAge = 14;
    public const int MaxAge = 125;

    public const int MaxCertDelay = 14;
}