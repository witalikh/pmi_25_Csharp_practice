using System.Text.Json;
namespace practice_task_1;

public partial class Menu<TObject>
    where TObject : class, IGenericValueType<string>, new()
{
    private readonly Collection<string, TObject> _innerCollection;
    private readonly Dictionary<string, string> _messages;
    private string? _fileName;

    private readonly TObject _emptySample = new();
    private AbstractUser user;

    public Menu(string msgFileName)
    {
        _innerCollection = new Collection<string, TObject>();
        _fileName = null;

        using StreamReader r = new(msgFileName);
        string json = r.ReadToEnd();
        try
        {
            _messages = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ??
                        new Dictionary<string, string>();
        }
        catch (JsonException)
        {
            _messages = new Dictionary<string, string>();
            Console.WriteLine("DebugInfo: There is some issue with file for messages. \n" +
                              "Menu might look corrupted");
        }
    }

    private void _PrintMessage(string key, params object?[]? s)
    {
        if (_messages.ContainsKey(key))
        {
            Console.WriteLine(_messages[key], s);
        }
        else
        {
            Console.WriteLine(key, ": ", s);
        }
    }

    private void _PrintErrors(ref ErrorsDict errors)
    {
        Console.WriteLine("!!!");
        foreach ((string field, var listOfErrors) in errors)
        {
            _PrintMessage($"{field}");
            foreach (string error in listOfErrors)
            {
                Console.WriteLine(_messages.ContainsKey(error) ? $" - {_messages[error]}" : $" - {error}");
            }
        }
        Console.WriteLine("!!!");
    }

    private string? _ChooseField()
    {
        string[] keys = _emptySample.Keys();
        
        _PrintMessage("ChooseFieldMenu");
        for (uint i = 0; i < keys.Length; ++i)
        {
            Console.Write(_messages.ContainsKey(keys[i])
                        ? _messages[keys[i]]
                        : keys[i]);
            Console.WriteLine($"-> {i}");
        }
        string option = Console.ReadLine() ?? string.Empty;

        try
        {
            int index = Convert.ToInt32(option.Trim());
            if (0 <= index && index < keys.Length)
                return keys[index];

            return null;
        }
        catch
        {
            return null;
        }
    }

    private void PrintMenu()
    {
        if (_fileName is null)
        {
            _PrintMessage("NotOpenedFile");
        }
        else
        {
            _PrintMessage("OpenedFile", _fileName);
        }
        _PrintMessage("Menu");
    }

    private void _PrintObject(TObject cert)
    {
        var itemsDict = cert.FancyItems();
        
        Console.WriteLine("---");
        foreach ((string key, string? value) in itemsDict)
        {
            Console.WriteLine(_messages.ContainsKey(key) ? $"{_messages[key]}{value}" : $"{key}: {value}");
        }
        Console.WriteLine("+++");
    }
    
    private void Sort()
    {
        string? field = _ChooseField();

        if (field == null)
        {
            _PrintMessage("WrongField");
            return;
        }
        
        _innerCollection.Sort(field);
        _PrintMessage("SuccessSort");
    }

    private void Clear()
    {
        if (_innerCollection.Size == 0)
        {
            _PrintMessage("AlreadyClean");
        }
        else
        {
            _innerCollection.Clear();
            _PrintMessage("SuccessClean");
        }
    }

    private bool RunOption(string option)
    {
        switch (option)
        {
            case "-1":
            case "exit":
            case "quit":
                return false;
            case "0":
                PrintMenu();
                break;
            case "1":
                OpenFile();
                break;
            case "2":
                CloseFile();
                break;
            case "3":
                LoadData();
                break;
            case "4":
                DumpData();
                break;
            case "5":
                PrintAll();
                break;
            case "6":
                PrintFiltered();
                break;
            case "7":
                Add();
                break;
            case "8":
                Edit();
                break;
            case "9":
                Delete();
                break;
            case "10":
                Sort();
                break;
            case "11":
                Clear();
                break;
            default:
                _PrintMessage("WrongQuery");
                break;
        }

        return true;
    }

    public void Run()
    {
        PrintMenu();
        bool running = true;
        while (running)
        {
            Console.Write(_messages.ContainsKey("ChooseOption")
                ? _messages["ChooseOption"]
                : "ChooseOption");
            string? option = Console.ReadLine();

            if (option != null)
                running = RunOption(option.Trim().ToLowerInvariant());
        }
    }
}