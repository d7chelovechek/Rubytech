using System.Text.Encodings.Web;
using System.Text.Json;

namespace Rubytech.Json.SerializationOptions
{
    public static class RubytechWriteSerializationOptions
    {
        public static JsonSerializerOptions Value => _value;

        private static readonly JsonSerializerOptions _value = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }
}