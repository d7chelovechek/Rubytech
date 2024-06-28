using Rubytech.Json.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rubytech.Json.Converters
{
    /// <summary>
    /// Json конвертер для получение <see cref="long"/> из <see cref="string"/>.
    /// </summary>
    public class StringToLongConverter : JsonConverter<long>
    {
        public override long Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            // Если токен строка и мы можем конвертировать её в число.
            if (reader.TokenType is JsonTokenType.String &&
                long.TryParse(reader.GetString(), out long value))
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