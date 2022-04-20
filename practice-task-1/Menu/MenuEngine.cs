using System.Text.Json;
namespace practice_task_1;

public partial class Menu<TObject>
    where TObject : class, IGenericValueType<string>, new()
{
    private readonly Collection<string, TObject> _innerCollection;
    private readonly Dictionary<string, string> _messages;
    private readonly Dictionary<string, AbstractUser> _users = new();

    private string? _fileName;

    private readonly TObject _emptySample = new();
    private AbstractUser user = MainRoles.AnonymousUser;

    private delegate void Option();

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
    
    private bool RunOption(string option)
    {
        Dictionary<string, Option> options;

        if (this.user is Staff)
        {
            options = new()
            {
                {"0", PrintMenu},
                {"1", PrintPublicAll},
                {"2", PrintPublicFiltered},
                {"3", PrintPrivateAll},
                {"4", PrintPrivateFiltered},
                {"5", Add},
                {"6", Edit},
                {"7", Delete},
                {"8", Sort},
                {"9", Logout}
            };
        } 
        else if (this.user is Admin)
        {
            options = new()
            {
                {"0", PrintMenu},
                // {"1", PrintAllUsers},
                {"2", PrintPublicAll},
                {"3", PrintDraftAll},
                {"4", PrintPublicFiltered},
                {"5", PrintDraftFiltered},
                {"6", Approve},
                {"7", Reject},
                {"8", Add},
                {"9", Edit},
                {"10", Delete},
                {"11", Sort},
                {"12", Logout}
                
            };
        }
        else
        {
            options = new()
            {
                {"0", PrintMenu},
                {"1", SignUp},
                {"2", SignIn}
            };
        }

        switch (option)
        {
            case "-1":
            case "exit":
            case "quit":
                return false;
            default:
                if (options.ContainsKey(option))
                {
                    options[option]();
                }
                else
                {
                    _PrintMessage("WrongQuery");
                }
                break;
        }

        return true;
    }

    public void Run()
    {
        PrintMenu();
        
        this.LoadUsers();
        this.LoadData();
        
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
        this.DumpData();
        this.DumpUsers();
    }
}