using System.Text.Json;
namespace practice_task_1;

public class Menu<TObject>
    where TObject : class,
    IRecognizable<string>, 
    IValidatable, 
    IFullyModifiable<Dictionary<string, string>>, 
    ILookupAble<string>,
    new()
{
    private readonly Collection<string, TObject> _innerCollection;
    private readonly Dictionary<string, string> _messages;
    private string? _fileName;

    private readonly TObject _emptySample = new();

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

    private void PrintObject(TObject cert)
    {
        var itemsDict = cert.Items();
        
        Console.WriteLine("---");
        foreach ((string key, string? value) in itemsDict)
        {
            Console.WriteLine(_messages.ContainsKey(key) ? $"{_messages[key]}{value}" : $"{key}: {value}");
        }
        Console.WriteLine("+++");
    }
    
    private void OpenFile()
    {
        _PrintMessage("FileOpenRequest");
        _fileName = Console.ReadLine();
    }

    private void CloseFile()
    {
        if (_fileName != null)
        {
            _PrintMessage("SuccessClose", _fileName);
            _fileName = null;
        }
        else
        {
            _PrintMessage("AlreadyClosed");
        }
    }
    
    private void LoadData()
    {
        if (_fileName != null)
        {
            try
            {
                var errors = _innerCollection.LoadFromJson(_fileName);
                _PrintMessage("SuccessLoad");

                if (errors.Size == 0) return;
                errors.DumpIntoJson(_fileName.Replace(".", "_") + ".json");
                _PrintMessage("LoadErrorsFound");
            }
            catch (JsonException)
            {
                _PrintMessage("FileCorruptedError");
            }
            catch (FileNotFoundException)
            {
                _PrintMessage("FileCorruptedError");
            }
        }
        else
        {
            _PrintMessage("FileNotSpecified");
        }
    }

    private void DumpData()
    {
        if (_fileName != null)
        {
            _innerCollection.DumpIntoJson(_fileName);
            _PrintMessage("SuccessDump");
        }
        else
        {
            _PrintMessage("FileNotSpecified");
        }
    }

    private void PrintAll()
    {
        _PrintMessage("PrintAllPrefix");
        for (int i = 0; i < _innerCollection.Size; ++i)
        {
            PrintObject(_innerCollection[i]);
        }
        _PrintMessage("Size", _innerCollection.Size);
    }

    private void PrintFiltered()
    {
        _PrintMessage("EnterFilterValue");
        string value = Console.ReadLine() ?? string.Empty;

        var filterResult = _innerCollection.Filter(value);
        
        _PrintMessage("PrintFilteredPrefix", value);
        for (int i = 0; i < filterResult.Size; ++i)
        {
            PrintObject(filterResult[i]);
        }
        _PrintMessage("Size", filterResult.Size);
    }

    private void Add()
    {
        string[] keys = _emptySample.Keys();
        var values = new Dictionary<string, string>();

        Console.WriteLine("+++");
        foreach (string property in keys)
        {
            Console.Write(_messages.ContainsKey(property) ? _messages[property] : property + ": ");
            values[property] = (Console.ReadLine() ?? string.Empty).Trim();
        }
        Console.WriteLine("---");

        TObject cert = new();
        cert.Modify(values);
        
        ErrorsDict errors = cert.GetValidationErrors();
        if (_innerCollection.Contains(cert.Id ?? string.Empty))
        {
            errors.Add("Id", "IdCollision");
        }
        
        if (errors.Count == 0)
        {
            _innerCollection.Add(cert);
            _PrintMessage("SuccessAdd");
        }
        else
        {
            _PrintErrors(ref errors);
        }
    }

    private void Edit()
    {
        _PrintMessage("EnterId");
        string id = Console.ReadLine() ?? string.Empty;

        if (!_innerCollection.Contains(id))
        {
            _PrintMessage("IdAbsent");
        }
        else
        {
            // find certificate index by id
            int certIndex = _innerCollection.GetIndex(id) ?? -1;
            
            // copy old data
            var certData = _innerCollection[certIndex].Items();
            
            // get field name to change
            string? field = _ChooseField();

            // invalid field
            if (field == null)
            {
                _PrintMessage("WrongField");
                return;
            }

            // valid field, ask value
            Console.Write(_messages.ContainsKey(field) ? _messages[field] : field + ": ");
            certData[field] = Console.ReadLine() ?? string.Empty;
            
            // form certificate with edited value
            TObject newCert = new();
            newCert.Modify(certData);
            ErrorsDict errors = newCert.GetValidationErrors();
            
            // if errors, print & exit
            if (errors.Count != 0)
            {
                _PrintErrors(ref errors);
                return;
            }
            
            // no errors, try to edit. fail => id collision
            _PrintMessage(!_innerCollection.Edit(id, newCert) ? "IdViolation" : "SuccessEdit");
        }
    }

    private void Delete()
    {
        // scan id
        _PrintMessage("EnterId");
        string id = Console.ReadLine() ?? string.Empty;

        // try to delete by id
        _PrintMessage(!_innerCollection.Remove(id) ? "IdAbsent" : "SuccessDelete");
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