using System.Text.Json;
namespace practice_task_1;

public partial class Menu<TObject>
{
    private void LoadData()
    {
        try
        {
            _innerCollection.LoadFromJson(FileName!, this._users);
        }
        catch (JsonException)
        {
            // TODO: some backup
            _PrintMessage("FileCorruptedError");
        }
        catch (FileNotFoundException)
        {
            
        }
    }

    private void DumpData()
    {
        _innerCollection.DumpIntoJson(FileName!);
    }
}