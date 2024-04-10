using System.Text.Encodings.Web;
using System.Text.Json;

namespace BertScout2024.Models;

public class BaseModel
{
    protected readonly JsonSerializerOptions WriteOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}