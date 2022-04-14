using System.Text.Json;
namespace practice_task_1;

public partial class Menu<TObject>
{
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

                // TODO: validation
                /*if (errors.Size == 0) return;
                errors.DumpIntoJson(_fileName.Replace(".", "_") + ".json");
                _PrintMessage("LoadErrorsFound");*/
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
}