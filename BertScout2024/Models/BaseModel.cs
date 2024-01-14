using System.Text.Encodings.Web;
using System.Text.Json;

namespace BertScout2024.Models;

public class BaseModel
{
    private readonly JsonSerializerOptions WriteOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, WriteOptions);
    }
}