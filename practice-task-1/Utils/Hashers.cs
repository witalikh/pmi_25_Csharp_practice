using System.Text;
using System.Security.Cryptography;
namespace practice_task_1;


public static class Hashers
{
    public static string ComputeSha256Hash(string rawData)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
        
        StringBuilder builder = new StringBuilder();  
        foreach (byte t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }  
        return builder.ToString();
    }
}