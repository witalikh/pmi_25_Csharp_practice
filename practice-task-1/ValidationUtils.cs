using System.Text.RegularExpressions;

namespace practice_task_1;

public static class ValidationUtils
{
    public static string? validate_regex(string? input, string pattern)
    {
        if (input != null && Regex.IsMatch(input.Trim(), pattern))
        {
            return input.Trim();
        }

        return null;
    }

    public static string? validate_choice(string? input, string[] choices)
    {
        return choices.Contains(input) ? input : null;
    }

    public static DateTime? try_to_datetime(dynamic? input)
    {
        if (input?.GetType().ToString() == "DateTime")
            return input;
        
        try
        {
            return Convert.ToDateTime(input);
        }
        catch (FormatException)
        {
            return null;
        }
    }
}