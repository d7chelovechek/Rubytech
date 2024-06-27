using Rubytech.Json.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rubytech.Json.Converters
{
    public class LongToStringConverter : JsonConverter<string>
    {
        public override string Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
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