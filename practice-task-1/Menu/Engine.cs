using practice_task_1.static_files;

namespace practice_task_1;

public partial class Menu<TObject>
    where TObject : class, IGenericValueType<string>, new()
{
    private readonly Collection<string, TObject> _innerCollection;
    private readonly Dictionary<string, string> _messages = Languages.English;
    private readonly Dictionary<string, AbstractUser> _users = new();

    private const string? FileName = "certificate.json";

    private readonly TObject _emptySample = new();
    private AbstractUser _user = MainRoles.AnonymousUser;

    private delegate void Option();

    public Menu()
    {
        _innerCollection = new Collection<string, TObject>();
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
        foreach ((string field, LinkedList<string> listOfErrors) in errors)
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
    
    private void _PrintObject(MetaDataWrapper<string, TObject> obj, bool printWithMeta = false)
    {
        TObject cert = obj.Value;
        Dictionary<string, string> itemsDict = cert.FancyItems();
        
        Console.WriteLine("---");
        foreach ((string key, string? value) in itemsDict)
        {
            Console.WriteLine(_messages.ContainsKey(key) ? $"{_messages[key]}{value}" : $"{key}: {value}");
        }

        if (printWithMeta)
        {
            Console.Write(" * ");
            this._PrintMessage("Draft", obj.Status.ToString());
            Console.Write(" * ");
            this._PrintMessage("Comment", obj.Comment);
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
        Dictionary<string, Option> options = this._user switch
        {
            Staff => new Dictionary<string, Option>
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
            },
            Admin => new Dictionary<string, Option>
            {
                {"0", PrintMenu},
                {"1", PrintAllUsers},
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
            },
            _ => new Dictionary<string, Option>
            {
                {"0", PrintMenu}, 
                {"1", SignIn}, 
                {"2", SignUp}
            }
        };

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