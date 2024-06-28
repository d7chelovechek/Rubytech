using Rubytech.Json.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rubytech.Json.Converters
{
    /// <summary>
    /// Json конвертер для получение <see cref="long"/>
    /// </summary>
    public class LongConverter : JsonConverter<long>
    {
        public override long Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            // Если токен число и мы можем его получить.
            if (reader.TokenType is JsonTokenType.Number &&
                reader.TryGetInt64(out long value))
            {
                return value;
            }

            throw new InvalidConvertibleValueException();
        }

        public override void Write(
            Utf8JsonWriter writer, 
            long value, 
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}