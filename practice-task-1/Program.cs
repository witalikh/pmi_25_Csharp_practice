// See https://aka.ms/new-console-template for more information

namespace practice_task_1;

internal static class Program
{
    public static void Main()
    {
        const string path = "lang_en.json";
        Menu<Certificate> menu = new(path);
        menu.Run();
    }
}