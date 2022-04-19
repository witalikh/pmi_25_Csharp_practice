using System.Text.Json;
namespace practice_task_1;

public partial class Menu<TObject>
{
    private void LoadData()
    {
        if (_fileName != null)
        {
            try
            {
                var errors = _innerCollection.LoadFromJson(_fileName);
                _PrintMessage("SuccessLoad");
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
        _innerCollection.DumpIntoJson("certificate");
    }
}