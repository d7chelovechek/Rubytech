using Rubytech.Json.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rubytech.Json.Converters
{
    /// <summary>
    /// Json конвертер для получение <see cref="string"/> из <see cref="long"/>.
    /// </summary>
    public class LongToStringConverter : JsonConverter<string>
    {
        public override string Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            // Если токен число и мы можем его получить.
            if (reader.TokenType is JsonTokenType.Number &&
                reader.TryGetInt64(out long value))
            {
                return value.ToString();
            }

            throw new InvalidConvertibleValueException();
        }

        public override void Write(
            Utf8JsonWriter writer, 
            string value, 
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}