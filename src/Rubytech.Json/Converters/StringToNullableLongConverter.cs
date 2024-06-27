using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rubytech.Json.Converters
{
    public class StringToNullableLongConverter : JsonConverter<long?>
    {
        public override long? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Number &&
                reader.TryGetInt64(out long value))
            {
                return value;
            }

            return null;
        }

        public override void Write(
            Utf8JsonWriter writer,
            long? value, 
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}