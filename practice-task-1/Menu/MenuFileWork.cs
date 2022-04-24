using System.Text.Json;
namespace practice_task_1;

public partial class Menu<TObject>
{
    private void LoadData()
    {
        try
        {
            _innerCollection.LoadFromJson(FileName!, this._users);
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

    private void DumpData()
    {
        _innerCollection.DumpIntoJson(FileName!);
    }
}