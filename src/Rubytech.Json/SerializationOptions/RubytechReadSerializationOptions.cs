using System.Text.Json;

namespace Rubytech.Json.SerializationOptions
{
    /// <summary>
    /// Настройки Json сериализатора для чтения.
    /// </summary>
    public static class RubytechReadSerializationOptions
    {
        /// <summary>
        /// Значение.
        /// </summary>
        public static JsonSerializerOptions Value => _value;

        private static readonly JsonSerializerOptions _value = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}