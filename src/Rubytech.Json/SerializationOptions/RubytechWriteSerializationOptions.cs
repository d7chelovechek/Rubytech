using System.Text.Encodings.Web;
using System.Text.Json;

namespace Rubytech.Json.SerializationOptions
{
    /// <summary>
    /// Настройки Json сериализатора для записи.
    /// </summary>
    public static class RubytechWriteSerializationOptions
    {
        /// <summary>
        /// Значение.
        /// </summary>
        public static JsonSerializerOptions Value => _value;

        private static readonly JsonSerializerOptions _value = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }
}