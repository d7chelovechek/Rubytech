using System.Text.Json;

namespace Rubytech.Json.SerializationOptions
{
    public static class RubytechJsonSerializationOptions
    {
        public static JsonSerializerOptions Value => _value;

        private static readonly JsonSerializerOptions _value = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}