using System.Text.Json;

namespace practice_task_1;

public class Menu
{
    private readonly Collection<string, Certificate> _innerCollection;
    private readonly Dictionary<string, string> _messages;
    private string? _fileName;

    public Menu(string msgFileName)
    {
        this._innerCollection = new Collection<string, Certificate>();
        this._fileName = null;

        using StreamReader r = new StreamReader(msgFileName);
        string json = r.ReadToEnd();
        _messages = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    }

    private void _PrintErrors(ref ErrorsDict errors)
    {
        Console.WriteLine("!!!");
        foreach ((string field, var listOfErrors) in errors)
        {
            Console.WriteLine(_messages[$"{field}"]);
            foreach (string error in listOfErrors)
            {
                Console.WriteLine($" - {_messages[error]}");
            }
        }
        Console.WriteLine("!!!");
    }

    private string? _ChooseField(string option)
    {
        string[] keys = Certificate.Keys();

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
        Console.WriteLine(_fileName == null ? 
            _messages["NotOpenedFile"] : 
            string.Format(_messages["OpenedFile"], _fileName));
        Console.WriteLine(_messages["Menu"]);
    }

    private void PrintCertificate(Certificate cert)
    {
        Dictionary<string, string> itemsDict = cert.Items();
        
        Console.WriteLine("---");
        foreach ((string key, string? value) in itemsDict)
        {
            Console.WriteLine($"{_messages[key]}{value}");
        }
        Console.WriteLine("+++");
    }
    
    private void OpenFile()
    {
        Console.WriteLine(_messages["FileOpenRequest"]);
        _fileName = Console.ReadLine();
    }

    private void CloseFile()
    {
        if (_fileName != null)
        {
            Console.WriteLine(_messages["SuccessClose"], _fileName);
            _fileName = null;
        }
        else
        {
            Console.WriteLine(_messages["AlreadyClosed"]);
        }
    }
    
    private void LoadData()
    {
        if (_fileName != null)
        {
            try
            {
                Collection<string, Certificate> errors = _innerCollection.LoadFromJson(_fileName);
                Console.WriteLine(_messages["SuccessLoad"]);

                if (errors.Size != 0)
                {
                    errors.DumpIntoJson(_fileName.Replace(".", "_") + ".json");
                    Console.WriteLine(_messages["LoadErrorsFound"]);
                }
            }
            catch (JsonException)
            {
                Console.WriteLine(_messages["FileCorruptedError"]);
            }
        }
        else
        {
            Console.WriteLine(_messages["FileNotSpecified"]);
        }
    }

    private void DumpData()
    {
        if (_fileName != null)
        {
            _innerCollection.DumpIntoJson(_fileName);
            Console.WriteLine(_messages["SuccessDump"]);
        }
        else
        {
            Console.WriteLine(_messages["FileNotSpecified"]);
        }
    }

    private void PrintAll()
    {
        Console.WriteLine(_messages["PrintAllPrefix"]);
        for (int i = 0; i < _innerCollection.Size; ++i)
        {
            PrintCertificate(_innerCollection[i]);
        }
        Console.WriteLine(_messages["Size"], _innerCollection.Size);
    }

    private void PrintFiltered()
    {
        Console.WriteLine(_messages["EnterFilterValue"]);
        string value = Console.ReadLine() ?? string.Empty;

        Collection<string, Certificate> filterResult = _innerCollection.Filter(value);
        
        Console.WriteLine(_messages["PrintFilteredPrefix"], value);
        for (int i = 0; i < filterResult.Size; ++i)
        {
            PrintCertificate(filterResult[i]);
        }
        Console.WriteLine(_messages["Size"], filterResult.Size);
    }

    private void Add()
    {
        string[] keys = Certificate.Keys();
        Dictionary<string, string> values = new Dictionary<string, string>();
        // string[] values = new string[keys.Length];
        
        Console.WriteLine("+++");
        foreach (string property in keys)
        {
            Console.Write(_messages[$"{property}"]);
            values[property] = (Console.ReadLine() ?? string.Empty).Trim();
        }
        Console.WriteLine("---");
        
        Certificate cert = new Certificate(values);
        
        ErrorsDict errors = cert.GetValidationErrors();
        if (_innerCollection.Contains(cert.Id ?? string.Empty))
        {
            errors.Add("Id", "IdCollision");
        }
        
        if (errors.Count == 0)
        {
            _innerCollection.Add(cert);
            Console.WriteLine(_messages["SuccessAdd"]);
        }
        else
        {
            _PrintErrors(ref errors);
        }
    }

    private void Edit()
    {
        Console.WriteLine(_messages["EnterId"]);
        string id = Console.ReadLine() ?? string.Empty;

        if (!_innerCollection.Contains(id))
        {
            Console.WriteLine(_messages["IdAbsent"]);
        }
        else
        {
            // find certificate index by id
            int certIndex = _innerCollection.GetIndex(id) ?? -1;
            
            // copy old data
            Dictionary<string, string> certData = _innerCollection[certIndex].Items();
            
            // get field name to change
            Console.WriteLine(_messages["ChooseFieldMenu"]);
            Console.Write(_messages["ChooseFieldOption"]);
            string option = Console.ReadLine() ?? string.Empty;
            string? field = _ChooseField(option);

            // invalid field
            if (field == null)
            {
                Console.WriteLine(_messages["WrongField"]);
                return;
            }

            // valid field, ask value
            Console.Write(_messages[field]);
            certData[field] = Console.ReadLine() ?? string.Empty;
            
            // form certificate with edited value
            Certificate newCert = new Certificate(certData);
            var errors = newCert.GetValidationErrors();
            
            // if errors, print & exit
            if (errors.Count != 0)
            {
                _PrintErrors(ref errors);
                return;
            }
            
            // no errors, try to edit. fail => id collision
            Console.WriteLine(!_innerCollection.Edit(id, newCert)
                ? _messages["IdViolation"]
                : _messages["SuccessEdit"]);
        }
    }

    private void Delete()
    {
        // scan id
        Console.WriteLine(_messages["EnterId"]);
        string id = Console.ReadLine() ?? string.Empty;

        // try to delete by id
        Console.WriteLine(!_innerCollection.Remove(id) 
            ? _messages["IdAbsent"] 
            : _messages["SuccessDelete"]);
    }

    private void Sort()
    {
        Console.WriteLine(_messages["ChooseFieldMenu"]);
        Console.Write(_messages["ChooseFieldOption"]);
        string option = Console.ReadLine() ?? string.Empty;
        string? field = _ChooseField(option);

        if (field == null)
        {
            Console.WriteLine(_messages["WrongField"]);
            return;
        }
        
        _innerCollection.Sort(field);
        Console.WriteLine(_messages["SuccessSort"]);
    }

    private void Clear()
    {
        if (this._innerCollection.Size == 0)
        {
            Console.WriteLine(_messages["AlreadyClean"]);
        }
        else
        {
            this._innerCollection.Clear();
            Console.WriteLine(_messages["SuccessClean"]);
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
                Console.WriteLine(_messages["WrongQuery"]);
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
            Console.Write(_messages["ChooseOption"]);
            string? option = Console.ReadLine();

            if (option != null)
                running = RunOption(option.Trim().ToLowerInvariant());

        }
    }
}