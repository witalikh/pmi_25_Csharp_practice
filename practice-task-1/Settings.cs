namespace practice_task_1;

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